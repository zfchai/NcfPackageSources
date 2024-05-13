﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Helpers.Serializers;
using Senparc.Ncf.Core.Exceptions;
using Senparc.Xncf.PromptRange.Domain.Models.DatabaseModel;
using Senparc.Xncf.PromptRange.Models.DatabaseModel.Dto;

namespace Senparc.Xncf.PromptRange.Domain.Services
{
    public partial class PromptItemService
    {
        public async Task<string> ExportPluginsAsync(string fullVersion)
        {
            var item = await this.GetObjectAsync(p => p.FullVersion == fullVersion) ??
                       throw new NcfExceptionBase($"未找到{fullVersion}对应的提示词靶道");
            var rangePath = await this.ExportPluginWithItemAsync(item);

            return rangePath;
        }

        public async Task<string> ExportPluginsAsync(IEnumerable<int> rangeIds, List<int> ids)
        {
            var rangeFilePaths = new List<string>();
            foreach (var rangeId in rangeIds)
            {
                var pluginFilePath = await this.ExportPluginsAsync(rangeId, ids);
                rangeFilePaths.Add(pluginFilePath);
            }

            // 根据 rangeFilePaths， 找出他们公共父文件夹的路径
            var commonParentPath = this.FindCommonParentPath(rangeFilePaths);

            return commonParentPath;
        }

        private string FindCommonParentPath(IEnumerable<string> paths)
        {
            var splitPaths = paths.Select(p => p.Split(Path.DirectorySeparatorChar)).ToList();
            var minLen = splitPaths.Min(sp => sp.Length);

            var commonPath = new List<string>();
            for (var i = 0; i < minLen; i++)
            {
                var dir = splitPaths[0][i];
                if (splitPaths.All(sp => sp[i] == dir))
                {
                    commonPath.Add(dir);
                }
                else
                {
                    break;
                }
            }

            return Path.Combine(commonPath.ToArray());
        }

        /// <summary>
        /// 根据靶场 ID, 导出该靶场下所有的靶道，返回文件夹路径
        /// </summary>
        /// <param name="rangeId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<string> ExportPluginsAsync(int rangeId, List<int> ids)
        {
            // 根据靶场名，获取靶场
            var promptRange = await _promptRangeService.GetAsync(rangeId);

            // 获取输出的靶场的文件夹路径
            var rangePath = this.GetRangePath(promptRange);

            // 根据靶场名，获取靶道
            var promptItemList = await this.GetFullListAsync(
                p => p.RangeName == promptRange.RangeName
                     && (ids == null || ids.Contains(p.Id))
            );

            // //用版号作为key, 映射字典
            // var itemMapByVersion = promptItemList.ToDictionary(p => p.FullVersion, p => p);

            // // 提取出 T 的第一位，并分组
            // Dictionary<string, List<PromptItem>> itemGroupByT = promptItemList.GroupBy(p => p.Tactic.Substring(0, 1))
            //     .ToDictionary(p => p.Key, p => p.ToList());

            // 每个靶道都需要导出
            foreach (var item in promptItemList)
            {
                // // 找出最佳item
                // var bestItem = itemList.MaxBy(p => isAvg ? p.EvalAvgScore : p.EvalMaxScore);

                await ExportPluginWithItemAsync(item, rangePath);
            }

            return rangePath;
        }

        /// <summary>
        /// 导出指定的单个靶道，返回文件夹路径
        /// </summary>
        /// <param name="promptItem"></param>
        /// <param name="rangePath"></param>
        /// <returns></returns>
        public async Task<string> ExportPluginWithItemAsync(PromptItem promptItem, string rangePath = null)
        {
            var range = await _promptRangeService.GetAsync(promptItem.RangeId);

            rangePath ??= this.GetRangePath(range);

            #region 根据模板构造 Root 对象

            var data = new Root()
            {
                schema = 1,
                description = "Generated by Senparc.Xncf.PromptRange",
                execution_settings = new ExecutionSettings()
                {
                    Default = new Default()
                    {
                        max_tokens = promptItem.MaxToken,
                        temperature = promptItem.Temperature,
                        top_p = promptItem.TopP,
                        presence_penalty = promptItem.PresencePenalty,
                        frequency_penalty = promptItem.FrequencyPenalty,
                        stop_sequences = (promptItem.StopSequences ?? "[]").GetObject<List<string>>()
                    }
                },
                input_variables = new List<PromptInputVariable>()
            };

            //添加输入对象
            var inputVarialbes = promptItem.GetInputValiableObject();
            data.input_variables.AddRange(inputVarialbes.Select(z => new PromptInputVariable(z)));

            #endregion

            //  当前 plugin 文件夹目录，靶道名/别名
            var curPluginPath = Path.Combine(rangePath, promptItem.NickName ?? promptItem.FullVersion);
            if (!Directory.Exists(curPluginPath))
            {
                Directory.CreateDirectory(curPluginPath);
            }
            else
            {
                // 如果别名已经存在，就增加一个尾缀
                curPluginPath += $"_{DateTime.Now:yyyyMMddHHmmss}";
                Directory.CreateDirectory(curPluginPath);
            }

            // 完整的JSON文件路径
            // string jsonFullPath = Path.Combine(curPluginPath, "config.json");

            await using (var jsonFs = new FileStream(
                             Path.Combine(curPluginPath, "config.json"),
                             FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                jsonFs.Seek(0, SeekOrigin.Begin);
                jsonFs.SetLength(0); // 清空文件内容
                await using (var jsonSw = new StreamWriter(jsonFs, Encoding.UTF8))
                {
                    // 写入并且保持格式
                    await jsonSw.WriteLineAsync(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }

            // 同理，构造 skprompt.txt 文件，内容为content
            await using (var txtFs = new FileStream(
                             Path.Combine(curPluginPath, "skprompt.txt"),
                             FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                txtFs.Seek(0, SeekOrigin.Begin);
                txtFs.SetLength(0); // 清空文件内容
                await using (var jsonSw = new StreamWriter(txtFs, Encoding.UTF8))
                {
                    await jsonSw.WriteLineAsync(promptItem.Content);
                }
            }

            return rangePath;
        }

        /// <summary>
        /// 根据靶场，生成文件夹，并返回文件夹路径
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private string GetRangePath(PromptRangeDto range)
        {
            #region 根据靶场别名，生成文件夹

            // 有别名就用别名，没有就用靶场名

            // 先获取根目录
            var curDir = Directory.GetCurrentDirectory();

            var filePathPrefix = Path.Combine(curDir, "App_Data", "Files");


            // 生成文件夹
            var rangePath = Path.Combine(filePathPrefix, "ExportedPluginsTemp", $"{range.Alias ?? range.RangeName}_{range.RangeName}");

            if (Directory.Exists(rangePath))
            {
                // 如果存在，就先清理指定文件夹
                Directory.Delete(rangePath, true);
            }
            Directory.CreateDirectory(rangePath);

            #endregion

            return rangePath;
        }

        #region Inner Class

        class ExecutionSettings
        {
            [JsonProperty] public Default Default { get; set; }
        }

        class Default
        {
            public int max_tokens { get; set; }
            public float temperature { get; set; }
            public float top_p { get; set; }
            public float presence_penalty { get; set; }
            public float frequency_penalty { get; set; }
            public List<string> stop_sequences { get; set; }
        }

        class Root
        {
            public int schema { get; set; }
            public string description { get; set; }
            public ExecutionSettings execution_settings { get; set; }

            public List<PromptInputVariable> input_variables { get; set; }
        }

        class PromptInputVariable
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Default { get; set; }

            public PromptInputVariable(InputVariable inputVariable)
            {
                Name = inputVariable.Name;
                Description = inputVariable.Description;
                Default = inputVariable.Default?.ToString();
                //TODO: 添加更多
            }
        }

        #endregion

        public async Task UploadPluginsAsync(IFormFile uploadedFile)
        {
            #region 验证文件

            if (uploadedFile == null || uploadedFile.Length == 0)
                throw new NcfExceptionBase("文件未找到");
            // 限制文件上传的大小为 50M
            if (uploadedFile.Length > 1024 * 1024 * 50)
            {
                throw new NcfExceptionBase("文件大小超过限制（50 M）");
            }

            if (!uploadedFile.FileName.EndsWith(".zip"))
            {
                throw new NcfExceptionBase("文件格式错误");
            }

            #endregion

            #region 保存文件

            var toSaveDir = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Files", "toImportFileTemp");
            if (!Directory.Exists(toSaveDir))
            {
                Directory.CreateDirectory(toSaveDir);
            }

            // 文件保存路径
            var zipFilePath = Path.Combine(toSaveDir, uploadedFile.FileName);

            using (var stream = new FileStream(zipFilePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            #endregion



            // 读取 zip 文件
            using var zip = ZipFile.OpenRead(zipFilePath);

            //解压
            var extractDir = $"{zipFilePath}-{SystemTime.NowTicks}";
            zip.ExtractToDirectory(extractDir);

            //判断文件结构
            //假设为完整路径
            var topDirs = Directory.GetDirectories(extractDir);
            List<PromptItem> promptItems = new List<PromptItem>();
            foreach (var topDir in topDirs)
            {
                // 先创建靶场
                Console.WriteLine("topDir:" + topDir);
                var rangeAlias = Path.GetFileName(topDir);
                var promptRangeDto = await _promptRangeService.AddAsync(rangeAlias);

                var pluginDirs = Directory.GetDirectories(topDir);
                var tacticIndex = 0;
                foreach (var pluginDir in pluginDirs)
                {
                    var configFilePath = Path.Combine(pluginDir, "config.json");
                    var skpromptFilePath = Path.Combine(pluginDir, "skprompt.txt");

                    if (!File.Exists(configFilePath) && !File.Exists(skpromptFilePath))
                    {
                        //TODO：给出失败提示
                        continue;
                    }

                    var promptItemAlias = Path.GetFileName(pluginDir);
                    var promptItem = new PromptItem(promptRangeDto, promptItemAlias, ++tacticIndex);
                    promptItems.Add(promptItem);

                    if (File.Exists(configFilePath))
                    {
                        // 读取所有的文件为一个 string
                        await using Stream stream = new FileStream(configFilePath, FileMode.Open);
                        using StreamReader reader = new StreamReader(stream);

                        string text = await reader.ReadToEndAsync();

                        var rootConfig = text.GetObject<Root>();
                        var executionSettings = rootConfig.execution_settings!;
                        var variableDictJson = rootConfig.input_variables != null && rootConfig.input_variables.Count > 0
                            ? rootConfig.input_variables.ToDictionary(z => z.Name, z => "").ToJson()
                            : null;

                        promptItem.UpdateModelParam(
                            topP: executionSettings.Default.top_p,
                            maxToken: executionSettings.Default.max_tokens,
                            temperature: executionSettings.Default.temperature,
                            presencePenalty: executionSettings.Default.presence_penalty,
                            frequencyPenalty: executionSettings.Default.frequency_penalty,
                            stopSequences: executionSettings.Default.stop_sequences.ToJson(),
                            variableDictJson: variableDictJson
                        );
                    }

                    if (File.Exists(skpromptFilePath))
                    {
                        // 读取所有的文件为一个 string
                        await using Stream stream = new FileStream(skpromptFilePath, FileMode.Open);
                        using StreamReader reader = new StreamReader(stream);

                        string skPrompt = await reader.ReadToEndAsync();

                        promptItem.UpdateContent(skPrompt);

                        // 提取 prompt 请求参数
                        var pattern = @"\{\{\$(.*?)\}\}";//TODO: 支持更多格式

                        // 没有参数
                        if (!Regex.IsMatch(skPrompt, pattern))
                        {
                            continue;
                        }

                        MatchCollection matches = Regex.Matches(skPrompt, pattern);
                        Dictionary<string, string> varDict = new();
                        foreach (Match match in matches)
                        {
                            string varKey = match.Groups[1].Value;
                            varDict[varKey] = "";
                        }

                        promptItem.UpdateVariablesJson(varDict.ToJson(), "{{$", "}}");
                    }

                }
            }


            #region 老方法
            /*
            // #region 可以选择先解压

            // zip.ExtractToDirectory(Path.Combine(toSaveDir, zipFile.FileName.Split(".")[0]), true);

            // 解压文件
            // var unzippedFilePath = Path.Combine(toSaveDir, zipFile.FileName.Split(".")[0], "");
            // if (!Directory.Exists(unzippedFilePath))
            // {
            //     Directory.CreateDirectory(unzippedFilePath);
            // }

            // ZipFile.ExtractToDirectory(toSaveFilePath, unzippedFilePath, Encoding.UTF8, true);
            // ZipFile.ExtractToDirectory(zipFile.OpenReadStream(), unzippedFilePath, Encoding.UTF8, true);

            // #endregion

            // 开始读取
            Dictionary<string, PromptItem> zipIdxDict = new();
            int tacticCnt = 0;
            foreach (var curFile in zip.Entries)
            {
                // var curFilePath = Path.Combine(extractPath, entry.FullName);
                var curDirName = Path.GetDirectoryName(curFile.FullName)!;
                if (curDirName.Contains('/') || curDirName.Contains('\\'))
                {
                    throw new NcfExceptionBase($"{curFile.FullName}文件格式错误");
                }

                if (curFile.Name == "") // 是目录
                {
                    var promptItem = new PromptItem(promptRange, curDirName, ++tacticCnt);

                    zipIdxDict[curDirName] = promptItem;
                }
                else
                {
                    // var directoryName = curDirName!;
                    // if (directoryName.Contains('/') || directoryName.Contains('\\'))
                    // {
                    //     throw new NcfExceptionBase($"{curFile.FullName}文件格式错误");
                    // }

                    // 从缓存中读取
                    var promptItem = zipIdxDict[curDirName];

                    // 根据不同文件名，更新不同的字段
                    if (curFile.Name == "config.json") // 更新配置文件
                    {
                        // 读取所有的文件为一个 string
                        await using Stream stream = curFile.Open();
                        using StreamReader reader = new StreamReader(stream);

                        string text = await reader.ReadToEndAsync();


                        var executionSettings = text.GetObject<Root>().ExecutionSettings!;

                        promptItem.UpdateModelParam(
                            topP: executionSettings.Default.TopP,
                            maxToken: executionSettings.Default.MaxTokens,
                            temperature: executionSettings.Default.Temperature,
                            presencePenalty: executionSettings.Default.PresencePenalty,
                            frequencyPenalty: executionSettings.Default.FrequencyPenalty,
                            stopSequences: executionSettings.Default.StopSequences.ToJson()
                        );
                    }
                    else if (curFile.Name == "skprompt.txt")
                    {
                        // 读取所有的文件为一个 string
                        await using Stream stream = curFile.Open();
                        using StreamReader reader = new StreamReader(stream);

                        string skPrompt = await reader.ReadToEndAsync();

                        promptItem.UpdateContent(skPrompt);

                        // 提取 prompt 请求参数
                        var pattern = @"\{\{\$(.*?)\}\}";

                        // 没有参数
                        if (!Regex.IsMatch(skPrompt, pattern))
                        {
                            continue;
                        }

                        MatchCollection matches = Regex.Matches(skPrompt, pattern);
                        Dictionary<string, string> varDict = new();
                        foreach (Match match in matches)
                        {
                            string varKey = match.Groups[1].Value;
                            varDict[varKey] = "";
                        }

                        promptItem.UpdateVariablesJson(varDict.ToJson());
                    }
                    else
                    {
                        continue;
                        throw new NcfExceptionBase($"{curFile.FullName}不符合上传要求");
                    }
                }
            }

            await this.SaveObjectListAsync(zipIdxDict.Values.ToList());
            */
            #endregion


            // 保存
            await this.SaveObjectListAsync(promptItems);
        }
    }
}

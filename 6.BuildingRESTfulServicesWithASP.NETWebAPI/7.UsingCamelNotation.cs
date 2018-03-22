//Q: 什么要使用CamelNotation？
//A: 现在API返回的粗数据全是PascalNotation，即每个字母的首写都是大写，这是.NET的约定习惯。但是JavaScript用的是CamelNotation，即首字母一定是小写，之后
  //每一个单词的首字母仍然是大写。我们需要将Pascal转换成Camel，才能让JavaScript更好的转换粗数据。

//Q: 如何让API输出CamelNotation格式的粗数据？
//A: 跟随以下步骤：
  //1.打开App_Start文件夹下的WebApiConfig.cs文件，即WebApi配置文件；
  //2.在其中的Register方法前排加入以下几条命令：

            var settings = config.Formatters.JsonFormatter.SerializerSettings;    //取得进行序列化设置的对象
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();   //将ContractResolver设置为Camel解析器
            settings.Formatting = Formatting.Indented;    //排版缩进

  //保存，这样API返回的就是CamelNotation格式的粗数据了。

//暂时想到这么多，最后更新，2018/03/21

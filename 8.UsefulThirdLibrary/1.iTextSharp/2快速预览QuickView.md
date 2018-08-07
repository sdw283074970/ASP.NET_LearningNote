# 快速预览 Quick View
本篇记录了如何设置iTextSharp环境以及iTextSharp的快速概览

### 如何安装iTextSharp
在Nugget.Net中搜索`iTextSharp`，然后选择第一个进行安装。
在项目目录的`引用`中添加`iTextSharp`。
在C#脚本中，通过添加`iTextSharp`命名空间添加引用。代码如下:

```c#
using iTextSharp.text;
using iTextSharp.text.pdf;
```

### 在iTextSharp中哪些类是常用类/核心类？
首先了解一下iTextSharp生成Pdf的步骤，如下：

1. 选择写入文件的方式。如果直接将PDF文件写入到硬盘中，声明文件流`FileStream`实例；如果写到内存中，则声明内存流`MemoryStream`实例；
2. 新建空白的PDF文件并打开；
3. 新建Pdf写入器`PdfWriter`；
4. 写入数据；
5. 关闭文件，关闭写入器，关闭内存流/文件流；
6. 如果是文件流写入，则生成的文件就在指定的目录下，如果是内存流写入，则文件在内存的缓冲区`buffer`中，需要对缓冲区操作导出。

所以，每一步涉及到的核心类如下：

1. `MemoryStream`类/`FileStream`类，都是非静态类，在`System.IO`命名空间下；
2. `Document`类，非静态类，在`iTextSharp.text`命名空间下；
3. `PdfWriter`类，非静态类，在`iTextSharp.text.pdf`命名空间下；
4. `Pharse`类、`Paragraph`类、`PdfPCell`类、`PdfPTable`类，都是非静态类，在`iTextSharp.text`命名空间下；
5. 无；
6. `HttpContext.Current`类，静态类，在`System.Web`命名空间下；

在掌握以上核心类后，就能输出最基本的PDF文件。

暂时想到这么多，最后更新2018/08/07

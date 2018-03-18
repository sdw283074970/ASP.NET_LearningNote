//Q: 为什么要为验证添加显示效果？
//A: 这是属于用户友好的一部分。将一些重要的信息标出来能提升用户的关注度。

//Q: 如何为验证添加显示效果？
//A: 反馈给用户的验证信息也是HTML元素，这意味着可以通过CSS文件对其进行渲染。如通过将某一些类作为属性应用到这些元素，来对验证失败的信息加粗、加红。
  //在ASP.NET中，可以通过在Site.css文件中对这些类进行声明。

  //如项目需求为对返回给读者的错误文字信息经行加红，对错误的输入表单部分经行红色描边。我们可以通过在Content文件夹下的Site.css文件中对对应类渲染。
  //项目中，视图中的错误信息由辅助函数Html.ValidationMessageFor()经行渲染。在生成的页面视图中，可以通过元素检测(Inspect)来查看网页元素的HTML代码。
    //例如，在Model定义中Name表单是Required的，如果为空就提交保存会获得"Name field is required."的验证错误通知。通过这个方法我们可以知道
    //"Name field is required."这条信息的类为class="field-validation-error"，因此我们只用在Site.css文件中对这个类进行声明渲染即可。CSS代码如下：

.field-validation-error{
    color: red;   //定义这个field的颜色为红色
}

  //同理，我们可以获得姓名输入表单元素的类为class="input-validation-error"，渲染其的CSS代码如下：

.input-validation-error{
    border: 2px solid red;    //围绕这个元素定义一个2像素的边框，并将颜色设为红色
}

  //再同理，我们可以为所有有经过Validation的元素、Model、对象应用CSS渲染。

//暂时想到这么多，最后更新2018/03/18

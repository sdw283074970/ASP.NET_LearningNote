//Q: 什么是在DataTable中使用ajax数据源？
//A: Ajax指异步JavaScript和XML，这里指DataTable使用在页面加载完毕后，动态向服务器API发送HTTP请求，以返回的粗数据(json)作为数据源，并对其进行渲染。

//Q: 为什么要使用ajax数据源？
//A: 因为ajax带来了效率。非ajax数据源，即服务器一开始就传输到客户端的XML文件是很大的，因为XML文件包含了视图中所有标签元素。XML文件本身很大不说，
  //DataTable在调用的过程中，会首先从XML文件中抽取有用的信息，再进行渲染。
  
  //这无疑会加大客户端的运算量，对性能造成无谓的损耗。DataTable是可以直接将json文件中的string数据准换成对象的，所以一开始用json文件作为数据载体是
    //最效率的，而且通过发送HttpGet请求给服务器Api就能得到json文件。

  //以上就是ajax技术在DataTable插件中扮演的角色：发送HttpGet请求，并本地渲染json数据。

//Q: 如何使用ajax技术获得和渲染数据源？
//A: 通过使用DataTable()方法中的可选参数即可。代码如下：

        $(document).ready(function () {
            $("#customers").DataTable({   //选择id为customers的表元素
                ajax: {   //第一个参数声明api的地址和数据源
                    url: "/api/customers",    //api地址
                    dataSrc: ""   //字符串指json数据中具体的数据源，空字符串意味着整个json数据都是数据源
                },
                columns: [    //第二个参数，声明了表列数列和列的内容，有三列就有三个对象
                    {
                        data: "name",   //第一个对象，数据为json数据中的"name"字段
                        render: function (data, type, customer) {   //将这个对象进行渲染，这里渲染成带超链接的"name"
                            return "<a href='/customer/edit" + customer.id +"'>" + customer.name + "</a>";
                        }
                    },
                    {
                        data: "name"    //第二个对象，为顾客的会员类型，但是此时会员类型为空，因为还没准备传输层次化的json数据，暂时返回"name"
                    },
                    {
                        data: "id",   //第三个对象，数据为id，然后渲染一个删除按钮，将id作为参数传给另一个jQuery方法(删除方法)
                        render: function (data) {
                            return "<button class='btn btn-link js-delete' data-customer-id=" + data + ">Delete</button>"
                        }
                    }
                ]
            });
            //jquery按钮删除方法省略
        });

  //如此以来，DataTable就会使用ajax传回的数据作为表列数据源，省时省力。这样原来的视图就可以删掉表内容<tbody>的部分，同样还可以删掉从控制器传来的
    //ViewModle视图，更进一步的，连控制器中的ViewModle也可以删掉，直接返回视图即可，因为数据现在来自于api控制器而不是普通控制器。但是表头<thead>
    //部分仍然保留，作为占位元素，否则DataTable渲染出来没有表头，不知所云。Index视图所有代码如下：

@{
    ViewBag.Title = "CustomerIndex";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Customer</h2>
<div>
    <div>
        <div>
            @Html.ActionLink("New Customer", "New", "Customers", null, new { @class = "btn btn-light"})
        </div>
    </div>
    <div>
        <table id="customers" class="table table-bordered table-hover">
            <thead>
                <tr>    //表头部分保留
                    <th>Customer</th>
                    <th>Membership Type</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>   //表中部分删除，有DataTable负责渲染
            </tbody>
        </table>
    </div>
</div>
@section scripts
{
    <script>
        $(document).ready(function () {   //本篇部分，使用ajax数据源渲染表中内容
            $("#customers").DataTable({
                ajax: {
                    url: "/api/customers",
                    dataSrc: ""
                },
                columns: [
                    {
                        data: "name",
                        render: function (data, type, customer) {
                            return "<a href='/customer/edit" + customer.id +"'>" + customer.name + "</a>";
                        }
                    },
                    {
                        data: "name"
                    },
                    {
                        data: "id",
                        render: function (data) {
                            return "<button class='btn btn-link js-delete' data-customer-id=" + data + ">Delete</button>"
                        }
                    }
                ]
            });

            $("#customers").on("click", ".js-delete", function () {   //第一篇部分，使用ajax和jquery定义一个删除按钮和方法
                var button = $(this);       //当前触发"click"事件的元素
                bootbox.confirm("Are u sure to delete?", function (result) {
                    if (result) {
                        $.ajax({
                            url: "/api/customers/" + button.attr("data-customer-id"),
                            method: "DELETE",
                            success: function () {
                                button.parents("tr").remove();
                            }
                        })
                    }
                });
            });
        });
    </script>
}

//暂时想到这么多，最后更新2018/04/02

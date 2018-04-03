//Q: 有什么问题？
//A: 当我们点击删除按钮，调用DELETE api并从数据库和表中删除掉一个记录，在不关闭应用的情况下，仍然可以在搜索栏中搜到刚才删掉的记录。
  //原因是，搜索栏是从一个内部表Internal Table中搜索对象的，我们仅仅是在数据库和DOM中删除了这个对象，而这个对象仍然存在与内部表，直到内存关闭。

  //我们应该从内部表中删除对象，再通知DOM与内部表同步。代码如下：

@section scripts
{
    <script>
        $(document).ready(function () {
            var table = $("#customers").DataTable({   //将选择的id为customers的元素声明一个内存引用保存
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
                        data: "memberShipType.memberShipTypeName",
                        render: function () {
                            return "<text>TEST</test>"
                        }
                    },
                    {
                        data: "id",
                        render: function (data) {
                            return "<button class='btn btn-link js-delete' data-customer-id=" + data + ">Delete</button>"
                        }
                    }
                ]
            });

            $("#customers").on("click", ".js-delete", function () {
                var button = $(this);       //当前触发"click"事件的元素
                bootbox.confirm("Are u sure to delete?", function (result) {
                    if (result) {
                        $.ajax({
                            url: "/api/customers/" + button.attr("data-customer-id"),
                            method: "DELETE",
                            success: function () {
                                //调用datatable中的api
                                table.row(button.parents("tr")).remove().draw();//row()即选取row, remove()即从内部表移除，draw()即同步内部表
                            }
                        })
                    }
                });
            });
        });
    </script>
}

//暂时想到这么多，最后更新2018/04/02

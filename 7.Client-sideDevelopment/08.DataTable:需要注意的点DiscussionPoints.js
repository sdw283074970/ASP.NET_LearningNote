//Q: 有什么需要注意的点？
//A: 第一个点是关于datatable插件的性能表现。目前，datatable通过ajax技术从api中获取粗数据，然后将数据储存在内部表中对其进行各种处理如排序、查找、分页。
  //这种方式能处理有几千条数据的表格没有问题，但是如果表中数据对象组成很肥大的话就会对性能产生影响。如，一个对象拥有一百条属性，这将极大的占用客户端
  //性能来处理内部表格。这种情况下，还是将处理数据的过程放在服务端，即api传出的数据已经是按顺序处理好的粗数据。

  //另一个问题，先看JaveScript代码：

$(document).ready(function () {
    var table = $("#customers").DataTable({
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
        var button = $(this);       
        bootbox.confirm("Are u sure to delete?", function (result) {
            if (result) {
                $.ajax({
                    url: "/api/customers/" + button.attr("data-customer-id"),
                    method: "DELETE",
                    success: function () {
                        table.row(button.parents("tr")).remove().draw();
                    }
                })
            }
        });
    });
});

  //以上代码看起来极其无序，第一个部分与视图有关，第二部分与数据处理有关，二者放一起会显著降低可维护性。正确的做法是将这两部分分别放在不同的js文件中，
    //需要的时候通过<script>标签分别调用。

//暂时想到这么多，最后更新2018/04/02

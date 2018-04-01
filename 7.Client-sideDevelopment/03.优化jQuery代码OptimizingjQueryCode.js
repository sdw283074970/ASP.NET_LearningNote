//Q: 目前的代码有什么好优化的？
//A: 目前添加事件方法on()有一些不效率。如果在一个页面有20个顾客信息，也就是说有20个删除按钮，这二十个按钮都是在id为customers的表下，且这些按钮
  //的类为 .js-delete。当前代码如下：
  
        $(document).ready(function () {
            $("#customers .js-delete").on("click", function () {
                var button = $(this);
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
        
  //选择器执行的顺序为：先选择id为customer的表，再在这个表中寻找类为 .js-delete的元素，为每个找到的元素添加事件方法on()。
  //这意味着，每一个删除按钮都有分开独立的句柄(handler)在内存中。等于说客户越多，句柄越多，占用的内存就越多。
  
  //解决方案为，使用添加事件方法on()的第二个可选参数on(event, childselector, data, function)中的childselector，即子选择器，于是可以有以下代码：

        $(document).ready(function () {
            $("#customers").on("click", ".js-delete", function () {   //使用第二个可选参数
                //内部代码...
            });
        });
        
  //这样一来，子选择器 ".js-delete"融入on()方法，最终只会生成一个customers的句柄存在于内存中，子选择器相当于成为了一个过滤器，所以最红效果不变。
    //整句句意为：选择id为customers的元素，为这个元素添加click方法，生成句柄，遍历这个元素的子元素，如果这个子元素为 .js-delete类，则执数function。
    
//暂时想到这么多，最后更新2018/04/01
  

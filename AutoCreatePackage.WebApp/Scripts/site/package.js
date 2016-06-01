var pageIndex = 1;
var pageCount = 1;
var allCount = 1;
var nextIndex = 0;
var prevIndex = 0;
var pageSize = 5;
var sortFlag = 0;
var searchName = null;

$(document).ready(function () {
    getPageSize();
    jumpPager();
    changeSortIcon();
    getSearchContent();
    loadData(1, pageSize, sortFlag, null);
    modalInit();
});


//加载数据
function loadData(p, s, sf, sn) {
    $.ajax({
        type: "GET",
        url: "/PackageInfo/GetPackageInfoList",
        data: "page=" + p + "&size=" + s + "&sort=" + sf + "&search=" + sn + "",
        beforeSend: function (XMLHttpRequest) {
            $("#p_tb").html("<div class='loader'><div class='loader-inner'><div class='loader-line-wrap'><div class='loader-line'></div></div><div class='loader-line-wrap'><div class='loader-line'></div></div><div class='loader-line-wrap'><div class='loader-line'></div></div><div class='loader-line-wrap'><div class='loader-line'></div></div><div class='loader-line-wrap'><div class='loader-line'></div></div></div></div>");
        },
        success: function (response) {
            if (response.status == "ok") {
                pageIndex = p;
                allCount = response.total;
                pageCount = Math.ceil(allCount / s);
                $("#allCount").text(allCount);
                $("#currentPage").text(p + "/" + pageCount);
                $("#homePage").attr("href", "javascript:loadData(1," + s + "," + sortFlag + "," + null + ")");
                $("#p_tb").empty();
                clearData();
                updatePagerBar();
                if (allCount == 0) {
                    disablePagerBar();
                    alert("软件包未入库或软件包名字错误！");
                }
                var list = "";
                $.each(response.rows, function (i, item) {
                    var options = "<a data-toggle='collapse' class = 'btn btn-primary btn-xs' href='#" + item.Id + "collapse' aria-expanded='false' aria-controls='#" + item.Id + "collapse'>更多</a>&nbsp;&nbsp;<a data-modal='' class = 'btn btn-primary btn-xs' href='/PackageInfo/EditPackage/" + item.Id + "'>编辑</a>&nbsp;&nbsp;<a data-modal='' class = 'btn btn-primary btn-xs' href='/PackageInfo/DeletePackage/" + item.Id + "'>删除</a>";
                    var status = "";
                    switch (item.PStatus) {
                        case 0:
                            status = "<span class='glyphicon glyphicon-time'></span><span> 等待更新</span>";
                            break;
                        case 1:
                            status = "<span class='glyphicon glyphicon-thumbs-up'></span><span style='color:green'> 更新成功</span>";
                            break;
                        case 2:
                            status = "<span class='glyphicon glyphicon-thumbs-down'></span><span style='color:red'> 更新失败</span>";
                            break;
                    }
                    var moreInfo = "<div><td colspan='7'><table class='table table-bordered'><tr><th style='width:200px'>软件包下载页面地址：</th><td>" + item.PDownLoadUrl + "</td></tr><tr><th style='width:200px'>软件包的XPath：</th><td>" + item.PXPath + "</td></tr><tr><th style='width:200px'>软件包SHA1:</th><td>" + item.PSHA1Code + "<a class = 'btn btn-primary btn-xs pull-right' href='" + item.PHtmlElementAttr + "'>下载最新的软件包</a></td></tr><tr><th style='width:200px'>软件包HtmlElementId:</th><td>" + item.PHtmlElementId + "</td></tr><tr><th style='width:200px'>软件包HtmlElementAttr:</th><td>" + item.PHtmlElementAttr + "</td></tr></table></td></div>";
                    list += "<tr><td>" + item.PId + "</td><td>" + item.PName + "</td><td>" + status + "</td><td>" + item.PCurrentVersion + "</td><td>" + ChangeDateFormat(item.PLastCheckDate) + "</td><td>" + ChangeDateFormat(item.PUpdateTime) + "</td><td>" + options + "</td></tr><tr class='collapse' id='" + item.Id + "collapse'>" + moreInfo + "</tr>";
                });
                $("#p_tb").append(list);
                modalInit();
            } else {
                alert("发生错误，稍后再试！");
            }
        }
    });
}

//改变时间格式
function ChangeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    commonTime = date.toLocaleString();
    return commonTime;
}

//获取每页显示大小
function getPageSize() {
    $("#pageSizeS").change(function () {
        pageSize = $("#pageSizeS").val();
        loadData(1, pageSize, sortFlag, null);
    });
}

//更新分页栏
function updatePagerBar() {
    if (pageIndex == "1") {
        $("#prevPage").attr("style", "color:gray");
    } else {
        prevIndex = eval(pageIndex) - eval("1");
        $("#prevPage").attr("href", "javascript:loadData(prevIndex,pageSize,sortFlag,null)");
    }
    if (pageIndex == pageCount) {
        $("#nextPage").attr("style", "color:gray");
        $("#lastPage").attr("style", "color:gray");
    } else {
        nextIndex = eval(pageIndex) + eval("1");
        $("#nextPage").attr("href", "javascript:loadData(nextIndex,pageSize,sortFlag,null)");
        $("#lastPage").attr("href", "javascript:loadData(pageCount,pageSize,sortFlag,null)");
    }
    for (var i = 1; i <= pageCount; i++) {
        if (i == pageIndex) {
            $("#pagerS").append("<option value ='" + i + "' selected='true'>" + i + "</option>");
        } else {
            $("#pagerS").append("<option value ='" + i + "'>" + i + "</option>");
        }
    }
}

//禁用分页栏
function disablePagerBar() {
    $("#homePage").attr("style", "color:gray");
    $("#homePage").removeAttr("href");
    $("#prevPage").attr("style", "color:gray");
    $("#prevPage").removeAttr("href");
    $("#nextPage").attr("style", "color:gray");
    $("#nextPage").removeAttr("href");
    $("#lastPage").attr("style", "color:gray");
    $("#lastPage").removeAttr("href");
}

//跳转页面
function jumpPager() {
    $("#pagerS").change(function () {
        loadData($("#pagerS").val(), pageSize, sortFlag, null);
    });
}

//清除分页栏属性和数据
function clearData() {
    $("#prevPage").removeAttr("style");
    $("#prevPage").removeAttr("href");
    $("#nextPage").removeAttr("style");
    $("#nextPage").removeAttr("href");
    $("#lastPage").removeAttr("style");
    $("#lastPage").removeAttr("href");
    $("#pagerS").empty();
}

//更改排序图标
function changeSortIcon() {
    $("#sortByUpdateTime").click(function () {
        sortFlag = $("#sortByUpdateTime").data('flag');
        if (sortFlag == 0) {
            $("#sortByUpdateTime").attr("class", "glyphicon glyphicon-sort-by-attributes");
            $("#sortByUpdateTime").data('flag', 1);
            sortFlag = 1;
        } else {
            $("#sortByUpdateTime").attr("class", "glyphicon glyphicon-sort-by-attributes-alt");
            $("#sortByUpdateTime").data('flag', 0);
            sortFlag = 0;
        }
        loadData(1, pageSize, sortFlag, null);
    });

}

//搜索方法
function getSearchContent() {
    $("#search").click(function () {
        searchName = $("#search_content").val();
        if (searchName.length > 0) {
            loadData(1, pageSize, sortFlag, searchName);
        } else {
            alert("请输入软件包名字");
        }
    });
}

//Modal部分
function modalInit() {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $("#acpModalContent").load(this.href, function () {
            $("#acpModal").modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, "show").on("shown.bs.modal", function (e) {
                $("#p_packageId").focus();
            });
            bindForm(this);
        });
        return false;
    });
}

function bindForm(dialog) {
    $("form", dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (response) {
                if (response.status == "ok") {
                    $("#acpModal").modal("hide");
                    $("#p_tb").empty();
                    loadData(1, pageSize, sortFlag, null);
                } else {
                    $("#acpModalContent").html(response);
                    bindForm();
                }
            }
        });
        return false;
    });
}
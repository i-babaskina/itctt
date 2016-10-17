
    //$("#dialog").dialog({ autoOpen: false });
    //$("#opener").click(function () {
    //    $("#dialog").dialog("open");
    //});

    var opt = {
        autoOpen: false,
        modal: true,
        width: 350,
        height: 350,
        title: 'Add good'
    };

var opt2 = {
    autoOpen: false,
    modal: true,
    width: 350,
    height: 500,
    title: 'Add movement'
};

var opt3 = {
    autoOpen: false,
    modal: true,
    width: 350,
    height: 160,
    title: 'Delete good?'
}

var opt4 = {
    autoOpen: false,
    modal: true,
    width: 450,
    height: 300
}


$("#opener").click(function ()
{
    $("#dialog").dialog(opt).dialog("open");
});


$("#deleteBtn").click(function () {
    $("#dialogDelete").dialog(opt3).dialog("open");
});


$("#showMovement").click(function ()
{
    $("#movementDialog").dialog(opt2).dialog("open");
});

$("#showStat").click(function () {
    $("#dialogStat").dialog(opt4).dialog("open");
    var myGrid = $('#jqList');
    var rowId = myGrid.jqGrid('getGridParam', 'selrow');
    console.log(rowId);
    var id = $('#' + rowId).children().first().text();
    console.log(id);
    $("#jqStat").jqGrid({
        url: '/Goods/GetAllMovementsForGood?goodId=' + id,
        datatype: "json",
        colNames: ['Operator', 'Operation', 'Date', 'Amount'],
        colModel: [
            { name: 'User', index: 'User', width: 100, sortable: true },
            { name: 'Type', index: 'Type', width: 100, sortable: true },
            { name: 'Date', index: 'Date', width: 150, sorttype: "date", sortable: true, date: true, formatter: myformatter},
            { name: 'Amount', index: 'Amount', width: 60, sortable: true },
        ],
        rowNum: 10,
        rowList: [10, 25, 50, 100],
        pager: '#pagerStat',
        sortname: 'Date',
        viewrecords: true,
        sortorder: "desc",
        loadonce: true,
        caption: "Good stats"
    });
    $("#jqStat").jqGrid('setGridParam', {
        url: '/Goods/GetAllMovementsForGood?goodId=' + id,
        datatype: 'json'
    }).trigger('reloadGrid');
});

function myformatter(cellvalue, options, rowObject) {
    new_formated_cellvalue = cellvalue.replace('T', ' ');
    var index = new_formated_cellvalue.lastIndexOf('.');
    if (index > 0)
        new_formated_cellvalue = new_formated_cellvalue.substring(0, index);
    return new_formated_cellvalue;
}


jQuery("#jqList").jqGrid({
    url: '/Goods/GoodsList',
    datatype: "json",
    loadonce: true,
    colNames: ['Id', 'Name', 'Price'],
    colModel: [
        { name: 'Id', index: 'Id', width: 25, sortable: true, hidden: true, editable: true },
        { name: 'Name', index: 'Name', width: 150, sortable: true, editable: true },
        {
            name: 'Price', index: 'Price', width: 150, sortable: true, editable: true,
            sorttype: function(cell, rowData) {
                return (parseInt(rowData.Price));
            }
        }
    ],
    rowNum: 10,
    rowList: [10, 25, 50, 100],
    pager: '#jqPager',
    pgbuttons: true,
    sortname: 'id',
    sortorder: "desc",
    onSelectRow: function(id) {
        $('#goodData').show();
        var rowData = $("#jqList").getRowData(id);
        //console.log(rowData);
        $('#goodName').html(rowData.Name);
        $('#goodPrice').html(rowData.Price);
        $('#movementGoodName').html(rowData.Name);
        $.ajax(
        {
            url: "/Goods/GetGoodDetails",
            type: "POST",
        data: rowData.Id,
        success: function(data, textStatus, jqXHR) {
            //console.log(data);
            $('#goodAmount').html(data.Amount);
            $('#goodAllPrice').html(data.TotalPrice);
        },
        error: function(jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        }
    });
},
    editurl: "/Goods/Update",
caption: "Goods"
});

jQuery("#jqList").jqGrid('navGrid', '#jqPager', { edit: false, add: false, del: false });

    //$("#addGoodForm").submit(function (e)
    //{
    //    var postData = $(this).serializeArray();
    //    console.log(postData);
    //    var formURL = $(this).attr("action");
    //    console.log(formURL);
    //    $.ajax(
    //    {
    //        url: formURL,
    //        type: "POST",
    //        data: postData,
    //        success: function(data, textStatus, jqXHR) {
    //            //alert("alertindex1");
    //            e.preventDefault();
    //            $('#jqList').setGridParam({ url: '/Goods/GoodsList', datatype: 'json', page: 1 }).trigger('reloadGrid');
    //            $('#newGoodName').val('');
    //            $('#newGoodPrice').val('');
    //            $('#refresh_jqList').click();
    //            $("#dialog").dialog(opt).dialog("close");
    //        },
    //        error: function(jqXHR, textStatus, errorThrown) {
    //            alert(textStatus);
    //        }
    //    });
    //    e.preventDefault(); //STOP default action

    //});
    //$("#addMovementForm").submit(function (e)
    //{
    //    // if (form.valid()) 
    //    {
    //        var postData = $(this).serializeArray();
    //        console.log(postData);
    //        var name = $('#movementGoodName').html();
    //        //postData["Name"] = name;
    //        postData.push({ name: "Name", value: name });
    //        console.log(postData);
    //        var formURL = $(this).attr("action");
    //        console.log(formURL);
    //        $.ajax(
    //        {
    //            url: formURL,
    //            type: "POST",
    //            data: postData,
    //            success: function (data) {
    //                console.log(data)
    //                if (data.success) {
    //                    $("#movementDialog").dialog(opt2).dialog("close");
    //                    e.preventDefault();

    //                }
    //                else {
    //                    $('#movementFormValidation').show();
    //                    $('#movementFormValidation').html('You can\'t take more goods then you have');
    //                    $('#amount').focus();
    //                    $('#amount').addClass(' form-control-danger');
    //                }
    //            },
    //            error: function (jqXHR, textStatus, errorThrown) {
    //                alert(textStatus);
    //            }
    //        });
    //        e.preventDefault(); //STOP default action
    //    }
    //});

$('#cancelMovement').click(function (e) {
    $("#movementDialog").dialog(opt2).dialog("close");
    $('#amount').removeClass(' form-control-danger');
    $('#movementFormValidation').hide();
    $('#movementFormValidation').html('');
});

//$("#cancelMovement").dialog({
//    close: function (event, ui) {
//        alert("close");
//        $("#movementDialog").dialog(opt2).dialog("close");
//        $('#amount').removeClass(' form-control-danger');
//        $('#movementFormValidation').hide();
//        $('#movementFormValidation').html('');
//    }
//});


jQuery.validator.addMethod(
    "money",
    function(value, element) {
        var isValidMoney = /^\d{0,10}(\.\d{0,2})?$/.test(value);
        return this.optional(element) || isValidMoney;
    },
    "Insert "
);


$().ready(function () {
    $("#addGoodForm").validate({
        rules: {
            Name: { required: true, maxlength: 50 },
            //TODO: only 9999 is max correct value. why??????
            Price: { required: true, money: true, max: 100000, min: 0.1 }
        },
        messages: {
            Name: { required: "enter a name", maxlength: "Maximum name length is 50 char" },
            Price: { required: "This field is required", money: "Invalid price format.", max: "Price must be less then 100000", min: "Price must be more then 0.1" }
        },
        submitHandler: function (form) {
            var postData = $(form).serializeArray();
            console.log(postData);
            var formURL = $(form).attr("action");
            console.log(formURL);
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    //alert("alertindex1");
                    if (data.success) {
                        //form.preventDefault();
                        $('#jqList').setGridParam({ url: '/Goods/GoodsList', datatype: 'json', page: 1 }).trigger('reloadGrid');
                        $('#newGoodName').val('');
                        $('#newGoodPrice').val('');
                        $('#refresh_jqList').click();
                        $("#dialog").dialog(opt).dialog("close");
                    }
                    else {
                        $('#goodFormValidation').show();
                        $('#goodFormValidation').html(data.responseText);
                        $('#name').focus();
                        $('#name').addClass(' form-control-danger');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }
    });
});


$().ready(function () {
    $("#loginForm").validate({
        rules: {
            Login: { required: true, maxlength: 255 },
            //TODO: only 9999 is max correct value. why??????
            Password: { required: true, maxlength: 255 }
        },
        messages: {
            Login: { required: "This field is required", maxlength: "Maximum login length is 255 char" },
            Password: { required: "This field is required", maxlength: "Maximum password length is 255 char" }
        },
        submitHandler: function (form) {
            var postData = $(form).serializeArray();
            console.log(postData);
            var formURL = $(form).attr("action");
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data, textStatus, jqXHR) {
                    console.log(data);
                    //alert("alertindex1");
                    if (data.success) {
                        window.location.replace("/Goods/Index");
                    }
                    else {
                        $('#loginFormValidation').html(data.message);
                        $('#loginFormValidation').show();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //alert("bad");
                    alert(textStatus);
                }
            });
        }
    });
});


$().ready(function () {
    $("#addMovementForm").validate({
        rules: {
            Amount: {required: true, max: 1000, digits: true}
        },
        messages: {
            Amount: { required: "Enter a number.", max: "Amount must be less then 1000.", digits: "Amount can be only integer." }
        },
        submitHandler: function (form) {
            var postData = $(form).serializeArray();
            //alert('smth');
            console.log(postData);
            var name = $('#movementGoodName').html();
            //postData["Name"] = name;
            postData.push({ name: "Name", value: name });
            console.log(postData);
            var formURL = $(form).attr("action");
            console.log(formURL);
            $.ajax(
            {
                url: formURL,
                type: "POST",
                data: postData,
                success: function (data) {
                    console.log(data)
                    if (data.success) {
                        $("#movementDialog").dialog(opt2).dialog("close");
                        form.preventDefault();

                    }
                    else {
                        $('#movementFormValidation').show();
                        $('#movementFormValidation').html('You can\'t take more goods then you have');
                        $('#amount').focus();
                        $('#amount').addClass(' form-control-danger');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
            //form.preventDefault();
        }
    });
});


    function EditRow() {
        var myGrid = $('#jqList');
        selectedRowId = myGrid.jqGrid('getGridParam', 'selrow');
        if (selectedRowId == null) alert('select row pleace');
        else {
            $("#jqList").jqGrid('editRow', selectedRowId);
            this.disabled = 'true';
            $("#saveBtn,#cancelBtn").attr("disabled", false);
            $("#saveBtn").removeClass("btn btn-default").addClass("btn btn-success");
        }
    }

function SaveRow() {
    var myGrid = $('#jqList');
    selectedRowId = myGrid.jqGrid('getGridParam', 'selrow');
    var id = $('#' + selectedRowId + "_Id").val();
    var newName = $('#' + selectedRowId + "_Name").val();
    var newPrice = $('#' + selectedRowId + "_Price").val();
    var postData = "id=" + id + "&Name=" + newName + "&Price=" + newPrice;
    $.ajax(
   {
       url: "/Goods/Update",
        type: "POST",
        data: postData,
        success: function (data, textStatus, jqXHR) {
            //alert(textStatus);
            $('#refresh_jqList').click();
            $('#jqList').setGridParam({ url: '/Goods/GoodsList', datatype: 'json', page: 1 }).trigger('reloadGrid');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        }
    });
$("#jqList").jqGrid('restoreRow', selectedRowId);
$("#saveBtn,#cancelBtn").attr("disabled", true);
$("#editBtn").attr("disabled", false);
$("#saveBtn").removeClass("btn btn-success").addClass("btn btn-default");

}

function CancelRow() {
    var myGrid = $('#jqList');
    selectedRowId = myGrid.jqGrid('getGridParam', 'selrow');
    $("#jqList").jqGrid('restoreRow', selectedRowId);
    $("#saveBtn,#cancelBtn").attr("disabled", true);
    $("#editBtn").attr("disabled", false);
    $("#saveBtn").removeClass("btn btn-success").addClass("btn btn-default");
}


function DeleteGood() {
    var myGrid = $('#jqList');
    selectedRowId = myGrid.jqGrid('getGridParam', 'selrow');
    $.ajax(
    {
        url: '/Goods/DeleteGood',
        type: "POST",
    data: selectedRowId,
    success: function (data, textStatus, jqXHR) {
        $('#goodsTable')./*trigger('reloadGrid').*/setGridParam({ url: '/Goods/GoodsList', datatype: 'json'/*, page: 1*/ }).trigger('reloadGrid');
        $('#refresh_jqList').click();
        $("#dialogDelete").dialog(opt3).dialog("close");
    },
    error: function (jqXHR, textStatus, errorThrown) {
        alert(textStatus);
    }
    });
}

$("#cancelDelete").click(function () {
    $("#dialogDelete").dialog(opt3).dialog("close");
});


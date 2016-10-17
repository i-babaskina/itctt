var goodMovementsUrl = '/Goods/GetAllMovementsForGood?goodId=';
var goodListUrl = '/Goods/GoodsList';
var goodDetailsUrl = "/Goods/GetGoodDetails";
var updateGoodUrl = "/Goods/Update";
var indexUrl = "/Goods/Index";
var deleteGoodUrl = '/Goods/DeleteGood';

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
        url: goodMovementsUrl + id,
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
        url: goodMovementsUrl + id,
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
    url: goodListUrl,
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
    onSelectRow: function (id) {
        $('#showMovement').prop('disabled', false);
        $('#showStat').prop('disabled', false);
        $('#deleteBtn').prop('disabled', false);
        $('#goodData').show();
        var rowData = $("#jqList").getRowData(id);
        //console.log(rowData);
        $('#goodName').html(rowData.Name);
        $('#goodPrice').html(rowData.Price);
        $('#movementGoodName').html(rowData.Name);
        $.ajax(
        {
            url: goodDetailsUrl,
            type: "POST",
        data: rowData.Id,
        success: function(data, textStatus, jqXHR) {
            //console.log(data);
            $('#goodPrice').html(data.Price);
            $('#goodAmount').html(data.Amount);
            $('#goodAllPrice').html(data.TotalPrice);
        },
        error: function(jqXHR, textStatus, errorThrown) {
            alert(textStatus);
        }
    });
},
    editurl: updateGoodUrl,
caption: "Goods"
});

jQuery("#jqList").jqGrid('navGrid', '#jqPager', { edit: false, add: false, del: false });


$('#cancelMovement').click(function (e) {
    $("#movementDialog").dialog(opt2).dialog("close");
    $('#amount').removeClass(' form-control-danger');
    $('#movementFormValidation').hide();
    $('#movementFormValidation').html('');
});


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
            Price: { required: true, money: true, max: 10000, min: 0.1 }
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
                    if (data.success) {
                        //form.preventDefault();
                        $('#jqList').setGridParam({ url: goodListUrl, datatype: 'json', page: 1 }).trigger('reloadGrid');
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
                        window.location.replace(indexUrl);
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
            Amount: {required: true, min:1, max: 1000, digits: true}
        },
        messages: {
            Amount: { required: "Enter a number.", min: "Amount must be 1 or more", max: "Amount must be less then 1000", digits: "Amount can be only integer" }
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
        $('#' + selectedRowId + '_Name').wrap('<form id=\'updateGoodFormName\'></form>');
        $('#' + selectedRowId + '_Price').wrap('<form id=\'updateGoodFormPrice\'></form>');
        this.disabled = 'true';
        $("#saveBtn,#cancelBtn").attr("disabled", false);
        $("#saveBtn").removeClass("btn btn-default").addClass("btn btn-success");
        $("#editBtn").attr("disabled", true);
    }
}

var isNameValid = false, isPriceValid = false;
//$().ready(function () {
//    $('#updateGoodFormName').validate({
//        rules: {
//            Name: { required: true, maxlength: 50 }
//        },
//        messages: {
//            Name: { required: "enter a name", maxlength: "Maximum name length is 50 char" }
//        },
//        submitHandler: function (form) {
//            isNameValid = true;
//            alert(isNameValid);
//        }
//    });
//    $('#updateGoodFormPrice').validate({
//        rules: {
//            Price: { required: true, money: true, max: 10000, min: 0.1 }
//        },
//        messages: {
//            Price: { required: "This field is required", money: "Invalid price format.", max: "Price must be less then 100000", min: "Price must be more then 0.1" }
//        },
//        submitHandler: function (form) {
//            isPriceValid = true;
//            alert(isPriceValid);
//        }
//    });
//});

function SaveRow() {
    //$('#updateGoodFormName').submit();
    //$('#updateGoodFormPrice').submit();
    var myGrid = $('#jqList');
    selectedRowId = myGrid.jqGrid('getGridParam', 'selrow');
    var id = $('#' + selectedRowId + "_Id").val();
    var newName = $('#' + selectedRowId + "_Name").val();
    var newPrice = $('#' + selectedRowId + "_Price").val();
    var postData = "id=" + id + "&Name=" + newName + "&Price=" + newPrice;
    $.ajax(
   {
       url: updateGoodUrl,
        type: "POST",
        data: postData,
        success: function (data, textStatus, jqXHR) {
            //alert(textStatus);
            $('#jqList').setGridParam({ url: goodListUrl, datatype: 'json', page: 1 }).trigger('reloadGrid');
            $('#refresh_jqList').click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
            alert('some error when update good');
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
    var id = $('#' + selectedRowId).children().first().text();
    console.log(id);
    $.ajax(
    {
        url: deleteGoodUrl,
        type: "POST",
    data: id,
    success: function (data, textStatus, jqXHR) {
        //$('#goodsTable')./*trigger('reloadGrid').*/setGridParam({ url: '/Goods/GoodsList', datatype: 'json', page: 1 }).trigger('reloadGrid');
        $("#goodsTable").jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
        $("#dialogDelete").dialog(opt3).dialog("close");
        //$('#refresh_jqList').click();
    },
    error: function (jqXHR, textStatus, errorThrown) {
        alert(textStatus);
    }
    });
}

$("#cancelDelete").click(function () {
    $("#dialogDelete").dialog(opt3).dialog("close");
});


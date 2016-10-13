/// <reference path="C:\Users\Lenovo\Source\Repos\AssetManagement\AssetManagement.WebUI\Scripts/jquery-3.1.1.js" />

$('#closeID').click(function () {
    console.log('close clicked')
    $('#closeID').hide();
    $('#solutionID').slideDown();
});
$('#cancelID').click(function(){
    console.log('close clicked')
    $('#solutionID').slideUp();
    $('#closeID').slideDown();
});

$(document).ready(function(){
    $('#completeID').click(function () {
        console.log(data);
        console.log(ticketID);
        var data = $('#solution').val();
        $.ajax({
            cache: false,
            async: true,
            url: "/" + controller + "/Ticket/" + ticketID + "?solution=" + data,
            type: "POST",
            data: {data},
            datatype: "html",
            success: refresh()
        });
        function refresh() {
            location.reload();
            $('#solutionID').slideUp();
            $('.alert').show(500);
            console.log(data);
        }
    });

})
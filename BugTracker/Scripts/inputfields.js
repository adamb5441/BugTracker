
$('.dtable').DataTable();
$(".pastcomments").hide()
$("#showcom").show()
$("#showcom").click(() => {
    $(".pastcomments").toggle()
    $("#showcom").hide();
})
$(".comment").autogrow();
$("#cancel").click(function (e) {
    e.preventDefault();
    $("#txtarea").val("")
})

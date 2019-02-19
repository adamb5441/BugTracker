
    $.ajax({
        url: "/Home/TicketData",
    type: 'GET',
            success: function (result) {
        console.log(result);
    }
});

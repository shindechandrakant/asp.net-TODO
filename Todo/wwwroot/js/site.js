
function deleteTodo(_id) {
    
    $.ajax({
        url: "Todo/Delete",
        type: "POST",
        data: {
            id: _id
        },
        success: function () {
            window.location.reload();
        }
    });
}




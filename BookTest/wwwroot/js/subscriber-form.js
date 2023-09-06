$(document).ready(function () {
    $('#governorate').change(function () {
        var governorateId = $("#governorate option:selected").val();
         $.ajax({
            url: "/Subscribers/LoadAreas?governorateId=" + governorateId,
            type: "Post",
           
            success: function (data) {
                $("#areas").empty();
                $("#areas").append('<option value="' + "" + '">' + "Select an area..." + '</option>');
                $.each(data, function (i, item) {
                    $("#areas").append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            },
            error: function (data) {
                alert(data);
            }
        });

    });
});

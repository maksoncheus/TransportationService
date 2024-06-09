$(document).ready(() => {
    $(document).find('.delete').on('click', function (e) {
        let _this = $(this),
            context = _this.closest('.user')
        changeOrderStatus(context, 0)
    });
    $(document).find('.deleteUser').on('click', function (e) {
        let _this = $(this),
            context = _this.closest('.user')
        changeOrderStatus(context, 1)
    });
    $(document).find('.gotten').on('click', function (e) {
        let _this = $(this),
            context = _this.closest('.user')
        changeOrderStatus(context, 3)
    });
    $(document).find('.approved').on('click', function (e) {
        let _this = $(this),
            context = _this.closest('.user')
        changeOrderStatus(context, 4)
    });
    $(document).find('.work').on('click', function (e) {
        let _this = $(this),
            context = _this.closest('.user')
        changeOrderStatus(context, 5)
    });
    function changeOrderStatus(context, newStatus) {
        $.ajax({
            url: '/Manage/Order/ChangeStatus',
            type: 'post',
            data: {
                id: context.data('id'),
                newStatus: newStatus
            },
            success: function (response) {
                location.reload();
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
});
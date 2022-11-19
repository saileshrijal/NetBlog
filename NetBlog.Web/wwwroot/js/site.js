
$(document).ready(function () {
    $("#logoutBtn").on("click", function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You really want to logout",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Logout'
        }).then((result) => {
            if (result.isConfirmed) {
                this.form.submit()
                Swal.fire(
                    'Logged Out!',
                    'You are successfully logged out',
                    'success'
                )
            }
        })
    })
})
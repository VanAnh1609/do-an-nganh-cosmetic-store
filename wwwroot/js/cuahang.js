console.log("cuahang.js da chay");

document.addEventListener("DOMContentLoaded", function () {

    const tokenInput = document.querySelector(
        '#antiForgeryForm input[name="__RequestVerificationToken"]'
    );

    const buttons = document.querySelectorAll(".btn-yeu-thich");

    buttons.forEach(function (button) {

        button.addEventListener("click", async function () {

            const maSanPham = this.dataset.id;
            const daYeuThich =
                this.dataset.daYeuThich === "true";

            const url = daYeuThich
                ? "/CuaHang/BoYeuThichAjax"
                : "/CuaHang/ThemYeuThichAjax";

            const formData = new FormData();

            formData.append(
                "maSanPham",
                maSanPham
            );

            if (tokenInput) {
                formData.append(
                    "__RequestVerificationToken",
                    tokenInput.value
                );
            }

            this.disabled = true;

            try {

                const response = await fetch(url, {
                    method: "POST",
                    body: formData
                });

                console.log(
                    "Yeu thich status:",
                    response.status
                );

                if (!response.ok) {
                    alert(
                        "Không thể cập nhật yêu thích. Mã lỗi: "
                        + response.status
                    );

                    return;
                }

                if (daYeuThich) {

                    this.classList.remove("btn-danger");
                    this.classList.add("btn-outline-danger");

                    this.innerHTML =
                        "♡ Yêu thích";

                    this.dataset.daYeuThich =
                        "false";
                }
                else {

                    this.classList.remove(
                        "btn-outline-danger"
                    );

                    this.classList.add(
                        "btn-danger"
                    );

                    this.innerHTML =
                        "♥ Đã yêu thích";

                    this.dataset.daYeuThich =
                        "true";
                }

            }
            catch (error) {

                console.error(
                    "Lỗi yêu thích:",
                    error
                );

                alert(
                    "Có lỗi khi cập nhật yêu thích."
                );

            }
            finally {

                this.disabled = false;

            }

        });

    });

});
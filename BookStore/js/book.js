$(document).ready(function () {
    var bookJS = new BookJS();
})

class BookJS {
    static currentPage = "1";
    constructor() {
        this.FormMode = null;
        this.initEvents();
        this.loadData();
    }

    initEvents() {
        $('#btnAdd').click(this.btnAddOnClick.bind(this));
        $('#btnSua').click(this.btnSuaOnClick.bind(this));
        $('#btnXoa').click(this.btnXoaOnClick.bind(this));
        $('#btnOK').click(this.btnOKOnClick.bind(this));
        $('#btnHuy').click(this.btnHuyOnClick.bind(this));
        $('.title-close-btn').click(this.btnCloseOnClick.bind(this));
        $('input[required]').blur(this.checkRequied);
        $("table tbody").on("click", "tr", this.rowOnSelect);
        $(".pagination_section").on("click", "a", this.pagingOnSelect);
        $(".txtSearch").on("input", this.textChanged);
        $(".txtSearch").on("remove", this.textChanged);
    }

    loadData() {
        //lay du lieu tren server thong qua loi goi toi api service

        $.ajax({
            url: "/api/v1/books/page?pageNum=" + BookJS.currentPage,
            method: "GET",
            contentType: "application/json",
            //dataType: "plain/text",
        }).done(function (response) {
            $('.grid tbody').empty();
            $.each(response, function (index, item) {
                var trHTML = $(`<tr>
                        <td>`+ item.BookCode + `</td>
                        <td>`+ item.BookName + `</td>
                        <td>`+ item.Author + `</td>
                        <td>`+ item.Price + `</td>
                        <td>`+ item.GenreName + `</td>
                        <td>`+ item.CompanyName + `</td>
                        <td>`+ item.Mount + `</td>
                        <td>`+ item.CreateAt + `</td>
                        <td>`+ item.ModifiedAt + `</td>
                        <td>`+ item.isDeleted + `</td>
                    </tr>`);
                $('.grid tbody').append(trHTML);
            })
        }).fail(function (response) {

        })

        //load option genre
        $.ajax({
            url: "https://localhost:44323/getgenre",
            method: "GET",
            contentType: "application/json",
            //dataType: "plain/text",
        }).done(function (response) {
            $('.opt-TheLoai').empty();
            $.each(response, function (index, item) {
                var trHTML = $(`<option class="opt" value=` + item.GenreCode + `>` + item.GenreName + `</option>`);
                $('.opt-TheLoai').append(trHTML);
            })
        }).fail(function (response) {

        })

        //load option company
        $.ajax({
            url: "https://localhost:44323/getcompany",
            method: "GET",
            contentType: "application/json",
            //dataType: "plain/text",
        }).done(function (response) {
            $('.opt-NXB').empty();
            $.each(response, function (index, item) {
                var trHTML = $(`<option value=` + item.CompanyCode + `>` + item.CompanyName + `</option>`);
                $('.opt-NXB').append(trHTML);
            })
        }).fail(function (response) {

        })
    }

    btnAddOnClick() {
        this.FormMode = "add";
        this.showDialogDetail();
    }
    btnSuaOnClick() {
        debugger;
        var self = this;
        this.FormMode = "edit";
        //Lay du lieu da chon
        //Xac dinh sach da chon
        var bookCode = this.getBookCodeSelected();
        //Lay ma sach
        if (bookCode) {
            //hien thi
            this.showDialogDetail();
            //api de lay du lieu cua sach da chon
            $.ajax({
                url: "/api/v1/books/getbook/" + bookCode,
                method: "GET"
            }).done(function (book) {
                if (!book) {
                    alert("Không có sách với mã tương ứng.")
                } else {
                    //bindding du lieu
                    debugger;
                    $('#txtBookName').val(book[0]["BookName"]);
                    $('#txtAuthor').val(book[0]["Author"]);
                    $('#txtPrice').val(book[0]["Price"]);
                    $('.opt-TheLoai').val(book[0]["GenreCode"]);
                    $('.opt-NXB').val(book[0]["CompanyCode"]);
                    $('#txtMount').val(book[0]["Mount"]);
                    //chinh sua du lieu tren form
                }
            }).fail(function (res) {

            })

        } else {
            alert("Vui lòng chọn thông tin.")
        }
    }

    btnXoaOnClick() {
        debugger;
        var self = this;
        //Lay du lieu da chon
        //Xac dinh sach da chon
        var bookCode = this.getBookCodeSelected();
        //Lay ma sach
        if (bookCode) {
            //hien thi
            //api de lay du lieu cua sach da chon
            $.ajax({
                url: "/api/v1/books/deletebook?bookCode=" + bookCode,
                method: "PUT"
            }).done(function (res) {
                if (res) {
                    alert("Thành công.")
                } else {
                    alert("Có lỗi xảy ra.")
                }

                self.loadData();
            }).fail(function (res) {

            })

        } else {
            alert("Vui lòng chọn thông tin.")
        }
    }

    btnHuyOnClick() {
        this.hideDialogDetail();
        $('input[required]').removeClass('required-error');
        $('input[required]').removeAttr("title", "Bạn phải nhập thông tin này.");
    }

    btnCloseOnClick() {
        this.hideDialogDetail();
        $('input[required]').removeClass('required-error');
        $('input[required]').removeAttr("title", "Bạn phải nhập thông tin này.");
    }

    btnOKOnClick() {

        var self = this;
        var method = "";
        var url = "";
        //Validate

        var inputRequired = $('[required]');
        var isValid = true;
        $.each(inputRequired, function (index, input) {
            var valid = $(input).trigger("blur");
            if (isValid && valid.hasClass("required-error")) {
                isValid = false;
            }
        })
        //thu thap du lieu tren form
        if (isValid) {
            debugger;
            var valueTL = $(".opt-TheLoai").val();
            var element = $(".opt-TheLoai").find('option').each(function () {
                if ($(this).val() == valueTL)
                    valueTL = $(this).attr("value")
            })

            var valueNXB = $(".opt-NXB").val();
            var element = $(".opt-TheLoai").find('option').each(function () {
                if ($(this).val() == valueNXB)
                    valueNXB = $(this).attr("value")
            })
            var book = {};
            book.BookName = $('#txtBookName').val();
            book.Author = $('#txtAuthor').val();
            book.Price = $('#txtPrice').val();
            book.GenreCode = valueTL;
            book.CompanyCode = valueNXB;
            book.Mount = $('#txtMount').val();

            if (self.FormMode == "edit") {
                var bookCode = this.getBookCodeSelected();
                method = "PUT";
                url = "/api/v1/books/" + "putbook/" + bookCode;
            }
            else if (self.FormMode == "add") {
                method = "POST";
                url = "/api/v1/books";
            }
            $.ajax({
                url: url,
                method: method,
                data: JSON.stringify(book),
                contentType: "application/json",
                dataType: "json"
            }).done(function (res) {
                self.hideDialogDetail();
                self.FormMode = null;
                self.loadData();
                return;
            }).fail(function (res) {
            })
        }
    }

    checkRequied() {
        var value = this.value;
        if (!value) {
            $(this).addClass('required-error');
            //$('#txtBookName').focus();
            $(this).attr("title", "Bạn phải nhập thông tin này.");
        }
        else {
            $(this).removeClass('required-error');
            $(this).removeAttr("title", "Bạn phải nhập thông tin này.");
        }
    }

    rowOnSelect() {
        $(this).siblings().removeClass("row-selected");
        $(this).addClass("row-selected");
    }

    pagingOnSelect() {
        $(this).siblings().removeClass("active");
        $(this).addClass("active");

        var pageNum = $(".pagination_section a.active").text();
        BookJS.currentPage = pageNum;
        $.ajax({
            url: "/api/v1/books/page?pageNum=" + BookJS.currentPage,
            method: "GET",
            contentType: "application/json",
            //dataType: "plain/text",
        }).done(function (response) {
            $('.grid tbody').empty();
            $.each(response, function (index, item) {
                var trHTML = $(`<tr>
                        <td>`+ item.BookCode + `</td>
                        <td>`+ item.BookName + `</td>
                        <td>`+ item.Author + `</td>
                        <td>`+ item.Price + `</td>
                        <td>`+ item.GenreName + `</td>
                        <td>`+ item.CompanyName + `</td>
                        <td>`+ item.Mount + `</td>
                        <td>`+ item.CreateAt + `</td>
                        <td>`+ item.ModifiedAt + `</td>
                        <td>`+ item.isDeleted + `</td>
                    </tr>`);
                $('.grid tbody').append(trHTML);
            })
        }).fail(function (response) {

        })

    }

    showDialogDetail() {
        $('.dialog input').val(null);
        $('.dialog-modal').show();
        $('.dialog').show();
        $('#txtBookName').focus();
    }

    hideDialogDetail() {
        $('.dialog-modal').hide();
        $('.dialog').hide();
    }

    getBookCodeSelected() {
        var bookCode = null;
        var trSelected = $("#tbBookList tr.row-selected");
        if (trSelected.length > 0) {
            bookCode = $(trSelected).children()[0].textContent;
        }
        return bookCode;
    }

    getCode() {
        var code = $("#tbBookList tr.row-selected[bookCode]")
        return code.attr("bookCode");
    }
    //getpageNumSelected() {
    //    var pageNum = null;
    //    var aSelected = $(".pagination_section a.active");
    //    if (aSelected.length > 0) {
    //        pageNum = $(".pagination_section a.active").text();
    //    }
    //    return pageNum;
    //}
    textChanged() {
        var data = $(".txtSearch").val();
        if (data == "") {

            $.ajax({
                url: "/api/v1/books/page?pageNum=" + BookJS.currentPage,
                method: "GET",
                contentType: "application/json",
                //dataType: "plain/text",
            }).done(function (response) {
                $('.grid tbody').empty();
                $.each(response, function (index, item) {
                    var trHTML = $(`<tr>
                        <td>`+ item.BookCode + `</td>
                        <td>`+ item.BookName + `</td>
                        <td>`+ item.Author + `</td>
                        <td>`+ item.Price + `</td>
                        <td>`+ item.GenreName + `</td>
                        <td>`+ item.CompanyName + `</td>
                        <td>`+ item.Mount + `</td>
                        <td>`+ item.CreateAt + `</td>
                        <td>`+ item.ModifiedAt + `</td>
                        <td>`+ item.isDeleted + `</td>
                    </tr>`);
                    $('.grid tbody').append(trHTML);
                })
            }).fail(function (response) {

            })
            return;
        }
        $.ajax({
            url: "/api/v1/books/search?data=" + data,
            method: "GET",
            contentType: "application/json",
            //dataType: "plain/text",
        }).done(function (response) {
            $('.grid tbody').empty();
            $.each(response, function (index, item) {
                var trHTML = $(`<tr>
                        <td>`+ item.BookCode + `</td>
                        <td>`+ item.BookName + `</td>
                        <td>`+ item.Author + `</td>
                        <td>`+ item.Price + `</td>
                        <td>`+ item.GenreName + `</td>
                        <td>`+ item.CompanyName + `</td>
                        <td>`+ item.Mount + `</td>
                        <td>`+ item.CreateAt + `</td>
                        <td>`+ item.ModifiedAt + `</td>
                        <td>`+ item.isDeleted + `</td>
                    </tr>`);
                $('.grid tbody').append(trHTML);
            })
        }).fail(function (response) {

        })
    }
}
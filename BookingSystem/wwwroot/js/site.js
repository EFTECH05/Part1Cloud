// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showLogin() {
    document.getElementById("registration").classList.remove('active');
    document.getElementById("login").classList.add('active');
}
function showRegistation() {
    document.getElementById("registration").classList.add('active');
    document.getElementById("login").classList.remove('active');
}

function submit() {
    var email1 = document.getElementById("email1").value;
    var password1 = document.getElementById("password1").value;

    if (email1 === "" && password === "") {
        alert("Please fill all the fields");
    } else if (email1 === "") {
        alert("Please fill the email field");
    } else if (password1 === "") {
        alert("Please fill the password field");
    }
    else {
        alert("Login successfully done!");
        alert("Welcome");
        // window.location.href = '/Home/BookingPage'; // Redirect to booking page
    }

}

function submit2() {

    var email2 = document.getElementById("email2").value;
    var password2 = document.getElementById("password2").value;

    if (email2 === "" && password2 === "") {
        alert("Please fill all the fields");
    } else if (email2 === "") {
        alert("Please fill the email field");
    } else if (password2 === "") {
        alert("Please fill the password field");
    }
    else {
        alert("Login successfully done!");
        alert("Welcome");
        window.location.href = '/Home/Admin';
    }

}

function getRegistration() {
    var name = document.getElementById("name").value;
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;
    var surname = document.getElementById("surname").value;
    if (name !== "" && email !== "" && password !== "" && surname !== "") {
        alert("Registration successfully done!");
        alert("You can now Login");
    }
    if (name === "" && email === "" && password === "" && surname === "") {
        alert("Please fill all the fields");
    } else if (name === "") {
        alert("Please fill the name field");
    } else if (email === "") {
        alert("Please fill the email field");
    } else if (password === "") {
        alert("Please fill the password field");
    } else if (surname === "") {
        alert("Please fill the surname field");
    }



}

function getRegistration2() {
    var name = document.getElementById("name").value;
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;
    var surname = document.getElementById("surname").value;
    if (name !== "" && email !== "" && password !== "" && surname !== "") {
        alert("Registration successfully done!");
        alert("You can now Login");
    }
    if (name === "" && email === "" && password === "" && surname === "") {
        alert("Please fill all the fields");
    } else if (name === "") {
        alert("Please fill the name field");
    } else if (email === "") {
        alert("Please fill the email field");
    } else if (password === "") {
        alert("Please fill the password field");
    } else if (surname === "") {
        alert("Please fill the surname field");
    }



}

function reset() {
    document.getElementById("email").value = "";
    document.getElementById("password").value = "";
    document.getElementById("surname").value = "";
    document.getElementById("name").value = "";
}
function reset2() {
    document.getElementById("email1").value = "";
    document.getElementById("password1").value = "";
}
function reset3() {
    document.getElementById("email2").value = "";
    document.getElementById("password2").value = "";
}
function CreateVenue() {

    // window.Location.href = '/Home/MakeBooking';
    window.location.href = '/Home/CreateVenue';
}
function ViewVenue() {

    window.location.href = '/Home/ViewVenue';
}

function CreateEvent() {

    // window.Location.href = '/Home/MakeBooking';
    window.location.href = '/Home/CreateEvent';
}
function ViewEvent() {

    window.location.href = '/Home/ViewEvent';
}
function Booking() {

    // window.Location.href = '/Home/MakeBooking';
    window.location.href = '/Home/Booking';
}
function ViewBooking() {

    window.location.href = '/Home/ViewBooking';
}
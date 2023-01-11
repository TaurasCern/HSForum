const insertHeader = () => {
    let accHtml = `<a href="login.html">Sign in</a>`;

    if(!(localStorage.getItem(`token`) === null))
    {
        accHtml = `<a href="profile.html">Profile</a>`;
    }

    document.querySelector(`header`).innerHTML =
    `<div class="header-container">
        <a class="header-logo" href="index.html">HSForum</a>
        <div class="header-navbar">
            <a href="forum.html">Forum</a>
            <a href="contacts.html">Contacts</a>
            <a href="aboutus.html">About us</a>
            ${accHtml}
        </div>
    </div>`;
}
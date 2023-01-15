let type;
window.onload = () => {
    insertHeader();
 
    let params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    type = params.type;

    if(type != null){
        loadPosts(type);
        loadEventListeners();
    }
    else window.location.assign("index.html");
}

const loadEventListeners = () => {
    loadCreatePostButton();
}
const loadPosts = async (type) => {
    let response = await fetch(`http://localhost:5084/api/Post/${type}`, {
        method: `get`,
        headers: {
            'Accept': 'application/json',
        },
    })

    if(response.ok){
        let json = await response.json();
        if(json.length != 0){
            json.forEach(element => { 
                insertPost(document.querySelector(`.posts-container`), element) 
            });
        }
    }
    else console.log(response.status);
}

const insertPost = (container, post) => {
    container.innerHTML += `
    <div class="post">
        <a class="post-title" href="post.html?id=${post.postId}">
            ${post.title}
        </a>
        <div class="post-date">
            ${(new Date(post.createdAt)).toLocaleString()}
        </div>
    </div>
    `;
}

const loadCreatePostButton = () => {
    document.querySelector(`.post-open-button`).addEventListener(`click`, () => {
        if(document.querySelector(`.create-post-form`) === null){
            insertCreatePost();
        }
        else{
            document.querySelector(`.create-post-form`).remove();
        }
    });
};

const insertCreatePost = () => {
    document.querySelector(`.post-open-button`).outerHTML +=`
        <form class="create-post-form">
            <label for="title">Title:</label>
            <input type="text" name="title" id="title" autocomplete="off">

            <label for="content">Content:</label>
            <textarea name="content" id="content cols="10" rows="10"></textarea>

            <input type="button" name="post-button" id="post-button" value="Post">
        </form>
    `;
    loadPostButtonEventListener();
    loadEventListeners();
};

const loadPostButtonEventListener = () => {
    document.querySelector(`#post-button`).addEventListener(`click`, (e) => {
        e.preventDefault();
        post();
    })
};

const post = async () => {
    let formData = new FormData(document.querySelector(`.create-post-form`));
    let data = {};

    formData.forEach((value, key) => data[key] = value);

    let response = await fetch(`http://localhost:5084/api/Post`, {
        method: `post`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
        body: JSON.stringify({
            title: data.title,
            content: data.content,
            postType: type
        })
    })

    if(response.ok){
        insertPost(document.querySelector(`.posts-container`), await response.json())
    }
    else console.log(response.status);
};
let id;
let type;
window.onload = async () => {
    insertHeader();
 
    id = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    }).id;

    if(id === null){
        window.location.assign("index.html")  
    }

    await loadPost();

    document.querySelector(`#negative-vote`).addEventListener(`click`, () => {
        ratePost(false);
    });

    if(!(localStorage.getItem(`token`) === null)){
        loadCommentCeateContainer();
    }
}
const loadPost = async () => {
    let response = await fetch(`http://localhost:5084/api/Post/${id}`, {
        method: `get`,
        headers: {
            'Accept': 'application/json',
        },
    })

    if(response.ok){
        let json = await response.json();
        let del = formatDeletePostButton(json);
        let edit = formatEditButton(json);
        document.querySelector(`article`).innerHTML = `
        <div class="post-container">
            <div class="post">
                <div class="post-title">
                    ${json.title}
                </div>
                <div class="post-content">
                    ${json.content}
                </div>
                <div class="post-info">
                    <div class="vote-container">
                        <div class="rating">
                            ${json.rating}
                        </div>
                        <a id="positive-vote" onclick="ratePost(true)">+</a>
                        <a id="negative-vote" onclick="ratePost(false)">-</a>
                    </div>
                    <div class="post-date">
                        ${(new Date(json.createdAt)).toLocaleString()}
                    </div>
                </div>
                <div class="post-actions">
                ${del}${edit}
                </div>
            </div>
        </div>
        <div class="comments-container"></div>`;

        if(json.replies.length != 0){
            let commentsContainer = document.querySelector(`.comments-container`);
            json.replies.forEach(element => {
                commentsContainer.innerHTML += formatComment(element);
            });
        }       
    }
    else console.log(response.status);
}
const formatDeletePostButton = (post) => {

    let roles = localStorage.getItem(`roles`);
    if(roles != undefined && roles != null){
        roles.split(`,`);
    }
    else return ``;
    
    if(roles.includes(`Moderator`) 
    || roles.includes(`Admin`) 
    || localStorage.getItem(`id`) == post.userId){
        return `<a id="delete-button" onclick="deletePost()">Delete</a>`;
    }
    return ``;
}
const deletePost = async () => {
    let response = await fetch(`http://localhost:5084/api/Post/${id}`, {
        method: `delete`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
    })

    if(response.ok){
        window.location.assign(`forum.html`);
    }
    else console.log(response.status);
}
const formatEditButton = (post) => {
    if(localStorage.getItem(`id`) == post.userId){
        return `<a id="edit-button" onclick="editPost()">Edit</a>`;
    }
    return ``;
}
const editPost = () => {
    document.querySelector(`.post-title`).setAttribute(`contenteditable`, `true`);
    document.querySelector(`.post-content`).setAttribute(`contenteditable`, `true`);
    let editButton = document.querySelector(`#edit-button`);
    editButton.setAttribute(`onclick`, `updatePost()`);
    editButton.innerHTML = `Confirm`;
}
const updatePost = async () => {
    let response = await fetch(`http://localhost:5084/api/Post/Patch/${id}`, {
        method: `patch`,
        headers: {
            'Content-Type': `application/json`,
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`    
        },
        body: JSON.stringify([
            {
                op: `replace`,
                path: `title`,
                value: document.querySelector(`.post-title`).innerHTML.trimStart(` `).trimEnd(` `)
            },
            {
                op: `replace`,
                path: `content`,
                value: document.querySelector(`.post-content`).innerHTML.trimStart(` `).trimEnd(` `)
            }
        ])
    })

    if(response.ok){
        document.querySelector(`.post-title`).setAttribute(`contenteditable`, `false`);
        document.querySelector(`.post-content`).setAttribute(`contenteditable`, `false`);
        let editButton = document.querySelector(`#edit-button`);
        editButton.setAttribute(`onclick`, `editPost()`);
        editButton.innerHTML = `Edit`;
    }
    else console.log(response.status);
} 
const formatComment = (comment) => {
    return `
        <div class="comment" id="comment-${comment.replyId}">
            <div class="comment-text-container">
                <div class="comment-user-container">
                    <div class="comment-user"><a href="profile.html?id=${comment.userId}">${comment.username}</a></div>
                    <div class="comment-date">${(new Date(comment.createdAt)).toLocaleString()}</div>
                </div>
                <div class="comment-content">${comment.content}</div>
            </div>
            ${formatDeleteCommentButton(comment.replyId)}
            
        </div>
    `;
}
const formatDeleteCommentButton = (id) => {
    let roles = localStorage.getItem(`roles`).split(`,`);

    if(roles.includes(`Moderator`) 
    || roles.includes(`Admin`) 
    || localStorage.getItem(`id`) == id){
        return `<a class="delete-comment-button" id="comment-${id}" onclick="deleteComment(${id})">Delete</a>`;
    }
    return ``;
} 
const deleteComment = async (id) => {
    let response = await fetch(`http://localhost:5084/api/PostReply/${id}`, {
        method: `delete`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
    })

    if(response.ok){
        document.querySelector(`#comment-${id}`).remove();
    }
    else console.log(response.status);
}
const loadCommentCeateContainer = () => {
    document.querySelector(`.post-container`).outerHTML += `
        <form class="comment-form">
            <textarea type="text" name="content" id="content" autocomplete="off"></textarea>
            <input type="button" name="comment-button" id="comment-button" value="Comment">
        </form>
    `;

    document.querySelector(`#comment-button`).addEventListener(`click`, () => {
        postComment();
    })
}

const postComment = async () => {
    let formData = new FormData(document.querySelector(`.comment-form`));
    let data = {};

    formData.forEach((value, key) => data[key] = value);

    let response = await fetch(`http://localhost:5084/api/PostReply`, {
        method: `post`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
        body: JSON.stringify({
            content: data.content,
            postId: id,
        })
    })

    if(response.ok){
        let json = await response.json();
        document.querySelector(`.comments-container`).innerHTML += formatComment(json);   
    }
    else console.log(response.status);
};

const ratePost = async (isPositive) => {
    let response = await fetch(`http://localhost:5084/api/Rating`, {
        method: `post`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
        body: JSON.stringify({
            isPositive: isPositive,
            postId: id,
        })
    })
    if(response.status === 200){

        let json = await response.json();
        let ratingContainer = document.querySelector(`.rating`);
        let rating = parseInt(ratingContainer.innerHTML);
        if(json.wasAltered){
            ratingContainer.innerHTML = json.isPositive ? rating + 2 : rating - 2;
        }
    }
    else if(response.status === 201){
        let json = await response.json();
        let ratingContainer = document.querySelector(`.rating`);
        let rating = parseInt(ratingContainer.innerHTML);
        ratingContainer.innerHTML = json.isPositive ? rating + 1 : rating - 1;
    }
    else console.log(response.status);
}
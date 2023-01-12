let id;

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
        document.querySelector(`article`).innerHTML = `
        <div class="post-container">
            <div class="post">
                <div class="post-title">
                ${json.title}
            </div>
            <div class="post-content">
                ${json.content}
            </div>
            <div class="post-date">
                ${(new Date(json.createdAt)).toLocaleString()}
            </div>
            <div class="rating">
                ${json.rating}
            </div>
            </div>
            <div class="vote-container">
                <a id="positive-vote" onclick="ratePost(true)">+</a>
                <a id="negative-vote" onclick="ratePost(false)">-</a>
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
const formatComment = (comment) => {
    return `
        <div class="comment" id="${comment.replyId}">
            <div class="comment-content">${comment.content}</div>
            <div class="comment-date">${(new Date(comment.createdAt)).toLocaleString()}</div>
        </div>
    `;
}
const loadCommentCeateContainer = () => {
    document.querySelector(`.post-container`).outerHTML += `
        <form class="comment-form">
            <label for="content">Comment:</label>
            <input type="text" name="content" id="content" autocomplete="off">

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
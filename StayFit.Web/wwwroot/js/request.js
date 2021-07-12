export default async function request(url, method, body) {
    let options = {
        method,
    };

    if (body) {
        Object.assign(options, {
            body: JSON.stringify(body)
        });
    }

    let response = await fetch(url, options);

    let data = await response.json();

    return data;
}

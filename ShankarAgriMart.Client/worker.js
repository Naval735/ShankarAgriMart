export default {
  async fetch(request, env) {
    const response = await env.ASSETS.fetch(request);

    // Real files: JS, CSS, images, fonts, index.html, etc.
    if (response.status !== 404) {
      return response;
    }

    // Angular SPA fallback
    return env.ASSETS.fetch(
      new Request(new URL('/index.html', request.url), request)
    );
  }
};
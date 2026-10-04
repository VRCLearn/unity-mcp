// Keep the canonical repository URL easy to copy from the migration page.
document.querySelector("code").addEventListener("click", () => {
    const selection = window.getSelection();
    const range = document.createRange();
    range.selectNodeContents(document.querySelector("code"));
    selection.removeAllRanges();
    selection.addRange(range);
});

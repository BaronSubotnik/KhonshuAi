export function scrollToBottom(el) {
    if (el) el.scrollTo({ top: el.scrollHeight,  behavior: "smooth"});   
}
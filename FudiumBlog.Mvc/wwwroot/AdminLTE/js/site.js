function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);
    //charAt() ilk harfi alır.toupper büyütür.slice ile de 1.indexten itibaren olan kısımları alırız.
}
function convertToShortDate(dateString) {
    const shortDate = new Date(dateString).toLocaleDateString('en-US');
    return shortDate;
}
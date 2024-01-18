// Only use this method for dates that shouldn't handle timezones i.e DateOnly on the server
export function DateToStringAPIFormat(date: Date) {
    const year = date.getFullYear().toString();
    let month = (date.getMonth() + 1).toString();
    let day = (date.getDate()).toString();

    if (month.length == 1) {
        month = "0" + month;
    }

    if (day.length === 1) {
        day = "0" + day;
    }

    return year + "-" + month + "-" + day;
}
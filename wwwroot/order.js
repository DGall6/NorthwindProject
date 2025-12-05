document.addEventListener("DOMContentLoaded", function () {
    fetchOrders();
});
document.getElementById('orderFilter').addEventListener("change", (e) => {
    fetchOrders();
});
async function fetchOrders() {
    const filter = document.getElementById('orderFilter').value;
    const { data: fetchedOrders } = await axios.get(`../../api/order${filter}`);
    console.log(fetchedOrders);
    let order_rows = "";
    // Create one date object here to not need to create 800+ date objects
    const now = new Date();
    fetchedOrders.map(order => {
        const requiredDate = new Date(order.requiredDate);
        // If shippedDate exists, use it to create new date object. If not, make empty string
        const shippedDate = order.shippedDate ? new Date(order.shippedDate) : "";
        let css = 'completed';
        let orderStatus = 'Completed';
        if (!shippedDate)
        {
            if (requiredDate < now)
            {
                css = "overdue";
                orderStatus = "Overdue!!!"
            } else {
                css = 'in-progress';
                orderStatus = "In Progress"
            }
        } else {
            if (requiredDate < shippedDate){
                css = 'late';
                orderStatus = "Completed Late";
            }
        }
        // console.log(shippedDate)
        order_rows +=
        `<tr class="${css}">
            <td>${order.orderId}</td>
            <td>${orderStatus}</td>
            <td class="text-end">${shippedDate ? shippedDate.toLocaleDateString() : "Not Shipped"}</td>
            <td class="text-end">${requiredDate.toLocaleDateString()}</td>
        </tr>`;
    });
    document.getElementById('order_rows').innerHTML = order_rows;
};
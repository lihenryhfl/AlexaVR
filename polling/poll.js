var AWS = require('aws-sdk');

AWS.config.update({region: 'us-east-1'});

var docClient = new AWS.DynamoDB.DocumentClient();

var timestamp=new Date().toLocaleString();

var params={
        TableName: "escapeCommands"
    };

docClient.scan(params, function(err, data) {
    if (err) {
        console.error("Unable to query. Error:", JSON.stringify(err, null, 2));
    } else {
        console.log("Query succeeded.");
console.log(JSON.stringify(data, null, 2));
console.log(data.Items[(data.Items.length)-1]);
}});

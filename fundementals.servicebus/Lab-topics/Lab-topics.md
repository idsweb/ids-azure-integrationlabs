# Topics and subscriptions
You can create Topics then Subscriptions that have filters. When a filter matches the message is delivered to a subscription. If you wish to see all messages then create a default subscription.

If you create a subscription without any filters a $default subscription is created with a filter of 1=1 so all messages are picked up.

## Note of filter matches ##
The rule for the subsription filter can be specified with a valid JSON type and the message appliction properties allow you to use an object. However experimenting with the following gave these results.

|Application property|correlationFilterProperty|Message property (portal)|Match?|
|--------------------|-------------------------|-------------------------|------|
|int|int|int(unquoted)|false|
|int|int|int(unquoted)|false|
|int|int|int quoted i.e. string|string|

However, correlation properties can only be a true match so working with strings isnt a massive handicap so long as you to string first.

Also you can match on Label so rather than add an application property if you can access it you can specify a Label.
# TDD-Pillar-Checkout
This is the Checkout Order Total Kata, done for my Pillar interview. The Kata can be found at https://github.com/PillarTechnology/kata-checkout-order-total. It's description is as follows:
>You have been contracted to write part of a grocery point-of-sale system. Your job is to implement the business logic to calculate the pre-tax total price as items are scanned or entered at checkout.

Many items are sold at a per-unit price. For example, a can of soup sells for $1.89 each.

Some items are sold by weight. For example, 80% lean ground beef currently sells for $5.99 per pound. Bananas are currently $2.38 per pound.

Each week, some items are marked down. For example, the soup could be marked down 20 cents. In this case, it would sell for $1.69 during the week. Markdown prices must always be used in favor of the per-unit price during the sale. There are laws protecting the customer from false advertising.

Along with the markdowns, a set of specials are advertised each week. For example, the soup could be advertised as "buy one, get one free" or "buy two, get one half off" or "three cans for $5.00." Sometimes limits are placed on these specials. For example, "Buy two, get one free. Limit 6." Purchases not fitting the description of the special are sold at the per-unit price unless a markdown price has also been advertised.

# Prerequisites
This library was build with dotnet core 2.2, and the test project was built with ms-test in visual studio code.
Dotnet core 2.2 can be found at https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.203-windows-x64-installer.

# How to build and run the tests
Once you have pulled this project and installed dotnet core 2.2, open a console window in your tool of choice, navigate to the test project folder: `<your/path/to>/PointOfSale/PointOfSale.Tests`. Once there, simply typ either `dotnet watch run` or `dotnet watch test` to see the project's tests run. It will run a build and also execute the tests. Conversely, you could run `dotnet build` first, then use the watch command separately, but there is no need.

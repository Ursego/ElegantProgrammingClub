// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Reuse the same code multiple times instead of creating duplicate copies.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Whenever repetition is found in the code base, a good programmer should refactor it so the logic for handling a specific task or rule exists in only one place.
// This means moving shared logic into a single reusable code block—either a generic fragment within the same function or a new, dedicated function.
// This kind of careful code reuse minimizes bugs, strengthens code reliability, and is a key principle of defensive programming.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// The dangers of copy-and-paste
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Using copy-and-paste to solve similar problems results in code duplication.
// This means maintaining multiple versions of nearly identical code, each slightly modified for a specific use.
// Over time, these inconsistencies spread throughout the codebase, creating a maintenance nightmare.
// That is a clear violation of the DRY (Don't Repeat Yourself) principle, which is fundamental to good software engineering.

// Every line of code in an application must be maintained and can become a source of future bugs.
// Duplication unnecessarily bloats the codebase, creating more chances for bugs and adding accidental complexity.
// This extra bloat also makes it harder for developers to understand the whole system or to be sure that a change in one place doesn’t also need to be made in other places.

// Often, we have a piece of code that works well in one place, and then we need similar functionality elsewhere.
// It's tempting to copy, tweak, and move on. But every time we do that, we face two serious risks:

// 1. When requirements change, we must update all copies of the duplicated logic—not just the original. If we miss even one copy, bugs can creep in.
// And since the developer may not even know duplication exists (buried deep in hundreds of functions across the codebase), this is a very real threat.

// 2. If a bug is discovered in one of the duplicated blocks, we often fix the version that was tested—but "forget" the other similar fragments since we don't know that they exist.
// As a result, identical bugs can remain hidden in other parts of the application, potentially for years.

// On the other hand, if all logic is centralized in one place that serves multiple parts of the application, any fix or enhancement is automatically shared across all those parts.

// Another benefit: having the logic in one place reduces overall code length and keeps functions, consuming the normalized logic, smaller.

// I just used the word "normalized" because the idea is very similar to normalization in databases.
// In databases, shared data is moved into a separate table, and other tables reference it instead of storing duplicate copies.
// Here, we're applying the same concept normalizing the program code rather than data.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Avoid code duplication within a function.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Don’t write similar code more than once. Instead, handle the differences in a single, generic block.
// The following example was taken from an old Sybase Transact-SQL stored procedure I was enhancing:

// *** BAD code: ***

IF @lang = 'FR'
  SET @brok_dtl = 'Courtier: ' + @prov_cd + ' ' + right ('0000' + convert (varchar, @brok_no), 4)
               + '-' + right ('00' + convert (varchar, @brok_ofc_no), 3)
               + ' ' + @name + ' - ' + @street_addr_1 + ' ' + @city + ', ' + @province
ELSE
  SET @brok_dtl = 'Broker: ' + @prov_cd + ' ' + right ('0000' + convert (varchar, @brok_no), 4)
               + '-' + right ('00' + convert (varchar, @brok_ofc_no), 3)
               + ' ' + @name + ' - ' + @street_addr_1 + ' ' + @city + ', ' + @province

// First of all, this approach forces the reader to compare both fragments, trying to spot the differences—only to realize they are minimal.
// If I would be the author, I would write this way:

// *** GOOD code: ***

SET @brok_dtl = CASE WHEN @lang = 'FR' THEN 'Courtier: ' ELSE 'Broker: ' END
               + @prov_cd + ' ' + right ('0000' + convert (varchar, @brok_no), 4)
               + '-' + right ('00' + convert (varchar, @brok_ofc_no), 3)
               + ' ' + @name + ' - ' + @street_addr_1 + ' ' + @city + ', ' + @province

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Extract IDENTICAL code fragments into a new function and call it from the places where the code was originally located.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If the duplicated fragments are in classes that share a common ancestor, place the new function in that ancestor and call it from the child classes.
// If the classes don’t share a common ancestor, create a generic function in a third class—even if you need to create that class just for this one function.
// If you're working in a non-object-oriented language (like C or Transact-SQL), simply extract the code into a new function or stored procedure.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Extract SIMILAR code fragments (and merge functions with SIMILAR functionality) into a new, smart generic function—even if they aren’t exactly identical.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Then call this function from the places where the result is needed, supplying the unique, location-specific data.
// There are different ways to provide this unique data, for example:
// 
// 1. As arguments.
//      The specific data is passed directly as one or more arguments to the new generic function.
// 2. Through a return value from a dedicated function.
//      You can create that function only for the sake of returning the circumstances-dependent data to one universal algorithm.
//      If the original duplicated fragments were in classes that inherit from the same superclass, create the function in the superclass as `protected`.
//      Make it abstract if your programming language supports abstract methods.
//      If it doesn’t, implement the function at the ancestor level even though it makes no sense.
//      The code must only throw an exception to clearly indicate that the base version must never be called and is intended to be overridden.
// 3. Using an instance variable.
//      If the original duplicated fragments were in classes that inherit from the same superclass, create the instance variable in the superclass as `protected`.

// For example, we have two TypeScript functions (I realize this example is extremely idiotic, but it demonstrates the idea well):

function changeCaseToUpper(input: string): string {
  alert("Going to make it upper...");
  const transformed = input.toUpperCase();
  alert("Made it upper!");
  return transformed;
}

function changeCaseToLower(input: string): string {
  alert("Going to make it lower...");
  const transformed = input.toLowerCase();
  alert("Made it lower!");
  return transformed;
}

// We strain our intellect a little and give birth to a new generic function:

type CaseType = "upper" | "lower";

function changeCase(input: string, caseType: CaseType): string {
  alert(`Going to make it ${caseType}...`);
  const transformed = (caseType === "upper") ? input.toUpperCase() : input.toLowerCase();
  alert(`Made it ${caseType}!`);
  return transformed;
}

// Example usage with alert:
alert(changeCase("hello", "upper"));
alert(changeCase("WORLD", "lower"));

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use CHOOSE CASE to avoid duplication od SQL queries
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Many times I've seen several SQL SELECT statements following each other. They were very similar, almost identical, differing only in some small detail. Something like these:

// *** BAD code: ***

SELECT order_id,
       customer_id,
       order_date,
       order_total,
       5 AS discount_percentage
  FROM orders
 WHERE order_date > ADD_MONTHS(SYSDATE, -3)
   AND order_total BETWEEN 50 AND 99.99

UNION

SELECT order_id,
       customer_id,
       order_date,
       order_total,
       10 AS discount_percentage
  FROM orders
 WHERE order_date > ADD_MONTHS(SYSDATE, -3)
   AND order_total BETWEEN 100 AND 299.99

UNION

SELECT order_id,
       customer_id,
       order_date,
       order_total,
       15 AS discount_percentage
  FROM orders
 WHERE order_date > ADD_MONTHS(SYSDATE, -3)
   AND order_total >= 300;

// This is not an elegant solution, because a single query with a simple CHOOSE CASE expression would do the same work!

// *** GOOD code: ***

SELECT order_id,
       customer_id,
       order_date,
       order_total,
       CASE 
          WHEN order_total < 100 THEN 5
          WHEN order_total >= 300 THEN 15
          ELSE 10
       END AS discount_percentage
  FROM orders
 WHERE order_date > ADD_MONTHS(SYSDATE, -3)
   AND order_total >= 50;

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Duplication may be the root of all evil in software. Many principles and practices have been created for the purpose of controlling or eliminating it.
// Consider, for example, how object-oriented programming serves to concentrate code into base classes that would otherwise be redundant.
// Subroutines, structured programming, component-oriented programming, functional programming are all strategies for eliminating duplication from our source code.

// Of all programming principles, Don't Repeat Yourself (DRY) is one of the most fundamental.  
// It forms the foundation for many other well-known software development best practices and design patterns.  
// A developer who learns to spot duplication and eliminate it through proper abstraction and good practices will write much cleaner code than
//         one who keeps spreading unnecessary repetition throughout the application.

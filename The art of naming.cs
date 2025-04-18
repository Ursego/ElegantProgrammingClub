// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Give classes, functions, variables, and especially DB tables and fields meaningful, descriptive, self-explanatory names that clearly reveal their purpose.
// Names should make intent obvious and reduce the need for comments.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// A good name should tell a story—what a variable or field contains, or what service a class or method provides.
// If a name needs a comment to explain it, then it doesn’t reveal its intent. Well-chosen names make code easier to read, understand, and maintain.

// *** BAD code: ***

int days; // after last purchase

// *** GOOD code: ***

int daysAferLastPurchase;

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Include the "per what" detail.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Always include the “per what” information (using the words "Per" or "By") when naming variables, especially for values like rates, limits, or durations.
// For example, instead of naming a variable `maxRequests`, use `maxRequestsPerMinute` or `maxRequestsPerUser`.
// This makes it immediately clear what the value represents and avoids confusion.
// A name like `discount` tells you little; `discountPerItem` or `discountPerOrder` makes the intent obvious.
// Being precise in names reduces misunderstandings and improves code clarity.
// This method is especially simplifies writing and reading financial calculations!

// Avoid abstract words like "Actual," "Total," and "Real."
// They will drive you crazy later when you're trying to figure out what is "actual," what isn't, and what level the "total" refers to.
// Is the `totalCows` field a total per barn? Per farm? Per village? Per province? Per country? Per the universe?
// If it's per farm, then how will you name the total per village? It's also "totalCows"!
// These words are meaningless without context.
// That context might be obvious to the data architect at the moment of table design ("of course it's total per what I'm thinking about now!"), but it becomes unclear later.
// Often, it's hard to see that context just by looking at a variable buried in hundreds of lines of code. So don’t just write "total" — write "total PER WHAT"!

// *** BAD code: ***

totalSalary = actualSalary * totalHours;

// *** GOOD code: ***

salaryPerDay = salaryPerHour * hoursPerDay;

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use the word "by" (or "of") in names of hash tables / maps / dictionaries / associative arrays.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Just an example in Java:

private static final HashMap<String, String> capitalCityByProvince = new HashMap<>();
static {
   capitalCityByProvince.put(province.ONTARIO, "Toronto");
   capitalCityByProvince.put(province.QUEBEC, "Montreal");
   capitalCityByProvince.put(province.MANITOBA, "Winnipeg");
}

// See how clear the code becomes when a `<something>By<something>` variable is used:

String clientCapitalCity = capitalCityByProvince.get(clientProvince);

// You might argue that the same information is repeated, and a shorter name like `capitalCities` would be clear enough (since the key variable already shows the "per what" part):

String clientCapitalCity = capitalCities.get(clientProvince);

// However, this repetition can help prevent bugs—if you use a wrong key, it becomes immediately noticeable:

String clientCapitalCity = capitalCityByProvince.get(clientLastName); // by province, not by last name, take a break to rest!

// This is crucial in real-life projects, where many hash tables store complex business entities—often far less obvious than `Province` and `City`.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Only use precise names that raise no (or very few) questions, even if they end up being long!
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// It’s common practice to prefer short names—but sometimes it’s worth breaking that rule and using longer, real-English phrases.
// Do this only when truly needed. But if it makes the code easier to work with, go for it! Compare:

String mainFilter = this.getMainFilter(); // bad...

// vs.

String filterBySelectedRowInSummaryeSection = this.getFilterBySelectedRowInSummarySection(); // good!

// As you can see, variable names can tell a full story, so readers instantly understand what’s happening.
// Even if the line becomes long, you can always split it across two lines—it’s better than guessing what a method returns or spending extra time digging into its code.

// From the book "Clean Code":

// "Don't be afraid to make a name long. A long descriptive name is better than a short enigmatic name. A long descriptive name is better than a long descriptive comment. Use a 
// naming convention that allows multiple words to be easily read in the function names, and then make use of those multiple words to give the function a name that says what it does.
// Don't be afraid to spend time choosing a name. Indeed, you should try several different names and read the code with each in place.
// Modern IDEs like Eclipse or IntelliJ make it trivial to change names.
// Use one of those IDEs and experiment with different names until you find one that is as descriptive as you can make it.
// Choosing descriptive names will clarify the design of the module in your mind and help you to improve it.
// It is not at all uncommon that hunting for a good name results in a favorable restructuring of the code."

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Names that describe actions must clearly indicate whether the action is to be performed in the future or has already happened.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// When naming variables or table fields, don’t make readers guess the timing of the action.
// In one of my projects, two different tables had fields named `calc_method`.
// One of them stored the method to use for the next calculation, while the other stored the method already used in the last calculation.
// Why not name them `calc_method_to_use` and `calc_method_used`?
// That would make both the business logic and SQL much easier to understand.

// Boolean names made of a noun only (without a verb) are another common issue.
// What does a Boolean variable named `isCalculation` mean? Calculation... what? How should `if (isCalculation)` be interpreted?
// There’s no confusion if the variable is named `isCalculationDone`, `isCalculated`, or, conversely, `doCalculation`, `shouldBeCalculated`, or just `calculate`.

// So, the general advice is to use prefixes like `do...`, `...ToDo`, `perform...`, `execute...`, `...ToApply`, `should...` for actions that are planned or expected to happen,
//     and suffixes like `...Done`, `...Performed`, `...Executed`, `...Occurred`, `...Passed`, `...Applied` for actions that have already taken place.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Declare separate variables for each distinct requirement.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// This falls under a broader rule: don't be lazy! When declaring a variable, choose a name that clearly reflects its specific role in the program.
// Reusing the same variable for multiple purposes (“recycling”) creates confusion and can easily lead to bugs.

// *** BAD code: ***

DECLARE
   l_count INTEGER;
BEGIN
   l_count := list_of_books.COUNT;
   IF l_count > 0 THEN
      l_count := list_of_books(list_of_books.FIRST).page_count;
      analyze_book(l_count);
   END IF;
END;

// *** GOOD code: ***

DECLARE
   l_book_count INTEGER;
   l_page_count INTEGER;
BEGIN
   l_book_count := list_of_books.COUNT;
   IF l_book_count > 0 THEN
      l_page_count:= list_of_books(list_of_books.FIRST).page_count;
      analyze_book(l_page_count);
   END IF;
END;

// In the future, you can change how one variable is used without worrying about unintended effects elsewhere in the code.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Be consistent throughout the application. Don’t create different versions of a name for the same entity.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// When working with values retrieved from DB tables, name the corresponding variables exactly as the DB fields (just adjust them to the naming convention if necessary).
// For example, if the DB field is `emp_id`, then your variable should be `empId`—not `empNo`, `empNum`, `employeeNo`, `employeeNum`, or `employeeId`.

// However, if the DB field names are not clear enough, you can be more flexible—especially when dealing with financial calculations.
// For instance, if a DB field `total_hours` holds the number of hours an employee worked in a single day, it’s better to assign that value to a variable named `hoursPerDay`.
// Clarity is more important than strict consistency.
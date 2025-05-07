// This file contains tips whose descriptions are not long enough to warrant separate files in the https://github.com/Ursego/ElegantProgrammingClub repository.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Store returned values in variables before consuming them
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Avoid using values returned by expressions or user-defined functions directly as inputs to other expressions or functions.
// Instead, assign them to intermediate variables and use those variables in the rest of the method.

// There are at least two good reasons for this approach:

// 1. Breaking complex nested expressions into several simpler lines makes the code easier to read and understand.

// 2. It simplifies debugging — you can see the returned value. In the next code
if (age < this.getMinAllowedAge())...
// the value returned by the function is inaccessible. To define it, you are forced to STEP IN and investigate. But the next fragment would be easier to debug:
int minAllowedAge = this.getMinAllowedAge();
if (age < minAllowedAge)...

// These two reasons justify always storing a functions return value in a variable — even if the function is called only once.
// Of course, if it's called more than once, there's no question at all — using a variable eliminates code duplication.

// The idea of storing returned values in variables naturally brings us to the next rule:

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Avoid complex nested expressions
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Don't write more than one executable statement per line to eliminate calling developer-defined methods nested within each other.

// Good developers break down long, complex methods into several short, simple sub-methods (https://github.com/Ursego/ElegantProgrammingClub/blob/main/Keep%20functions%20simple.txt).
// The same principle applies to deeply nested expressions: break a long, complicated line of code into multiple short, clear lines.
// The code may become longer, but thats a small price to pay compared to wasting time deciphering an overly dense and convoluted statement.

// *** BAD code: ***

string countryName = this.getCountryName(this.getCountryId(this.getCityId(rowNum)));

// *** GOOD code: ***

int cityId = this.getCityId(rowNum);
int countryId = this.getCountryId(cityId);
string countryName = this.getCountryName(countryId);

// The last example shows how this approach simplifies debugging (in addition to improving readability): if the variable countryName isnt populated as expected, you can immediately see which specific step failed — without needing to STEP IN.
// You might notice that cityId or countryId hasn't been populated, and you'll immediately see which direction to take in your investigation.

// In the nested version (the BAD code), if you want to step into getCountryName to debug it, youre first forced to step into (and out of) each inner method, starting from the most deeply nested one (getCityId).

// Avoiding complex nested expressions is especially important in financial calculations.
// If the result of a multi-step computation is incorrect, it's crucial to identify which specific intermediate step contains the bug.
// More than once, Ive had to break down complicated expressions written by other developers into simpler ones just to figure out which step failed.
// So lets take care of the future in advance!

// HOWEVER...

// When I wrote "Don't write more than one executable statement per line to eliminate calling developer-defined methods nested within each other", I specifically meant functions written by you or your teammates.
// It doesnt make sense to introduce variables just to hold values returned from built-in or framework functions that have been tested millions of times and are absolutely predictable — doing so only makes the code longer without providing any benefit.
// The following lines of code are perfectly legitimate (I just copy-pasted them from the Java project I am working on right now, at the time of writing this text):

InputStream stream = getClass().getClassLoader().getResourceAsStream(FILE_NAME);
...
ZCRMModule module = ZCRMRestClient.getInstance().getModuleInstance(moduleName);
...
String brokerName = excelRow.getCell(EXCEL_CELL_NUM__BROKER_NAME).getStringCellValue();
...
if (excelRow.getCell(0).getStringCellValue().trim().isEmpty()) return;
...
MainApplication.getLogger().info("Converted " + recordsArrayList.size() + " rows from sheet " + sheet.getSheetName());

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Law of Demeter (or Principle of Least Knowledge)
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If you want to get data from an object, referenced by a variable, then use a public function, declared in the referenced object itself, but not variables and functions of its nested object(s).

// Let's read the Wikipedia (https://en.wikipedia.org/wiki/Law_of_Demeter):

// "...a given object should assume as little as possible about the structure or properties of anything else (including its subcomponents), in accordance with the principle of "information hiding"...
// ...an object should avoid invoking methods of an object returned by another method. For many modern object-oriented languages that use a dot as field identifier, the law can be stated simply as "use only one dot" That is, the code a.m().n() breaks the law where a.m() does not. As an analogy, when one wants a dog to walk, one does not command the dog's legs to walk directly; instead, one commands the dog which then commands its own legs."

// *** BAD code: ***

customerAge = this.order.getCustomer().getAge() // the current object inserts its "dirty hands" into internal mechanics of another object - order

// *** GOOD code: ***

customerAge = this.order.getCustomerAge() // the code of getCustomerAge() could be "return this.getCustomer().getAge();" or whatever - it's not the busines of the current object

// In both the cases we have the same result - the variable customerAge is populated with a same value.
// But there is a huge difference if we are talking about possible dangers! In the GOOD code, only the order object is responsible for its internal affairs.
// If the age's storage method is changed one day, it will be enough only to change the code of getCustomerAge(), and all the calls to that function all over the application will keep working without any change.

// Heres another real-life example — paying at the checkout in a supermarket.
// Once all items have been scanned and its time to pay, its up to you to decide how exactly you want to proceed.
// Most likely, youll take out your wallet and choose the appropriate card. Or maybe youll use cash. Or maybe not from your wallet at all, but from a different pocket.
// Your pay() method has been called, and the logic inside is your personal business — no one elses concern.
// Now imagine the cashier rudely pulling your wallet out of your pocket, picking whichever card they like, and completing the payment with it while you watch in disbelief!

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Declare variables with as narrow scope as possible
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If the value is used only within a single method, declare it as a local variable.
// If a value is used in multiple methods but doesn't need to persist between method calls, pass it as a parameter.

// Declare an instance or static variable only if the value needs to persist between method calls!

// The shorter the variable's lifecycle, the easier the code is to understand and debug, and the lower the risk of bugs.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Avoid unnecessary null checks
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// In various languages, there's a function or operator that takes two values and returns the first one if it's not NULL, and if it is NULL, it returns the second one.

// In stored procedures, this is usually a function:

// PL/SQL:
NVL(use_if_it_is_not_null, use_otherwise)
IsNull(use_if_it_is_not_null, use_otherwise)

// In many languages (for example, C# and TypeScript), it's ?? - the null-coalescing operator:

use_if_it_is_not_null ?? use_otherwise

// In the following examples, I will use NVL, but the same logic applies to IsNull and ??.

// I have often seen NVL used even when it's not necessary, which creates unnecessary and meaningless "visual noise", for example:

NVL(v_dept_id, 0) = i_dept_id
NVL(v_orders_qty, 0) > 0
NVL(v_order_status, '~') IN (pck_constants.ORDER_STATUS__COMPLETED, pck_constants.ORDER_STATUS__CANCELLED)

// Therefore, I advise colleagues to use the following rule:

// Check for nulls only when the expressions would produce a different result without those checkings.

// That makes the code cleaner (think about complex expressions!), not to mention performance.

// NVL doesn't make sense with =, >, <, =>, <= and IN. So, the correct way to write the last example is
v_my_var = i_dept_id
v_orders_qty > 0
v_order_status IN (pck_constants.ORDER_STATUS__COMPLETED, pck_constants.ORDER_STATUS__CANCELLED)

// If the var is NULL, the expression will return the same FALSE as with NVL.

// Of course, NVL must be used in the next situation:

// * To check an equality when NULL value is considered as a non-NULL value. A classical example is treating NULL as 'N' in 'N'/'Y' variables:
NVL(v_my_var, 'N') = 'N'

// * To check an ineqality (obviously, the second parameter to NVL must be a value illegal for the variables):
NVL(v_dept_id, 0) <> i_dept_id
NVL(v_order_status, '~') NOT IN (pck_constants.ORDER_STATUS__COMPLETED, pck_constants.ORDER_STATUS__CANCELLED)

// * To check an equality when two NULLs are considered an equal "value":
NVL(v_var1, '~') = NVL(v_var2, '~')



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



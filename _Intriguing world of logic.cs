// This file contains multiple tips related to Boolean operations.

// The table on content:

// * Populating Boolean variables
// * Short conditions in IFs
// * Boolean functions names
// * Positive wording in Boolean names
// * Use the NOT operator as little as possible
// * Use positive comparisons in Boolean expressions
// * Use short-circuit evaluation to improve performance

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Populating Boolean variables
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If possible, populate Boolean variables using the result of Boolean expressions instead of assigning true or false directly.

// *** BAD code: ***

if (rowCount > 1)
   multiRowsMode = true;
else
   multiRowsMode = false;

// *** GOOD code: ***

multiRowsMode = (rowCount > 1);

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Short conditions in IFs
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Don't write complex logical expressions directly in if statements.
// Instead, evaluate the expression beforehand, store the result in a Boolean variable with a clear, descriptive name, and use that variable in the if statement.
// This makes the logic behind the condition much easier to understand, as the code becomes self-explanatory.

// *** BAD code: ***

if ((calcMethod == CalcMethods.ADDITIVE && additiveCalculationPassed) || (calcMethod == CalcMethods.RATIO && ratioCalculationPassed))...

// Wow, that's hard to understand! Okay, maybe this example is easy to understand — but think about real-world logical expressions where you can't make heads or tails of it...
// I've seen if statements with half-a-screen-long Boolean expressions!

// However, if you use a Boolean variable to hide the expression's complexity, the result is a code that is virtually as readable as "straight" human language.
// The BAD code says only WHEN the condition is true, but the GOOD code tells us both WHEN and WHY:

// *** GOOD code: ***

boolean structuralChangeOccurred =
    (calcMethod == CalcMethods.ADDITIVE && additiveCalculationPassed) ||
    (calcMethod == CalcMethods.RATIO && ratioCalculationPassed);
       
if (structuralChangeOccurred)...

// Now, the if statement reads like plain English and clearly answers the Most Important Question of Programming: "What the hell is going on here?"
// If, one day, developers need to understand how the Boolean variable is computed, they can look under the covers.
// But, in most cases, a name like "structuralChangeOccurred" is enough to convey the high-level business logic.

// If the expression spans more than a few lines, you can consider refactoring it into a separate Boolean function and calling that function from the if.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Boolean functions names
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If a function returns a Boolean, its name should clearly convey the meaning of the returned value to make the business logic in the calling code easy to understand.
// When used as a condition in an if statement, the function call should read like a proper English sentence.

// Here are some examples of well-named Boolean methods: rowValidated, ordersOk, fileNameExists.
// There is a strange tradition is to start such names with words like "is", "are", "does", or "do" (e.g., isRowValidated, areOrdersOk, doesFileNameExist).
// That style produces awkward or grammatically incorrect expressions in the calling code — we say "if row is validated", not "if is row validated".
// Of course, this isn't a critical issue. You can even omit "is" or "are" entirely, and the names will still be clear: rowValidated, ordersOk.

// There's an exception to this rule — getters of Boolean fields (instance variables). These should follow the standard getField or isField naming convention.
// In Java, this is part of the JavaBean specification, and it makes sense not just for tools (which can infer field names from getters), but also for developers.
// For instance, the method name isInvoicePrinted suggests the existence of a Boolean field invoicePrinted in the class, while invoiceIsPrinted doesn't.

// Never use imperative verbs to name Boolean methods!

// Names like validateRow, checkOrders, or checkFileExistence are completely inappropriate for methods that return Boolean values.
// We don't say "if validate row then save", "if check orders then print", or "if check file existence then open file".
// I have no explanation for why I have encountered this practice countless times over my decades of working as a programmer.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Positive wording in Boolean names
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Give Boolean database fields, variables, and functions positive names.
// That makes Boolean expressions easier to read and understand, especially when they're nested.

// The reason for this is that your variables and functions may be used in complex expressions that include negation.
// Positive naming will result in one negation in the worst case, for example:
!docPrinted
// However, with negative naming, you may end up with "a negation of a negation" — a construct that makes the overall logic confusing:
!docNotPrinted
// That looks awkward and adds unnecessary clutter.

// You can already see the difference with simple expressions — now imagine them as part of a larger, more complex condition!

// A few examples (negative form to avoid → positive form to prefere):

// nonStandard → standard
// notFound → found, exists
// notReady → ready
// notRetrieved → retrieved

// Try not to use negative prefixes; instead, substitute them with the corresponding positive prefixes:

// excluded → included
// disabled → enabled

// Instead of using words with the in- and un- prefixes (such as invalid, incomplete, unavailable, unchecked, unconfirmed, unissued, unprotected, unsaved), use their non-prefixed antonyms (valid, complete, available, checked, confirmed, issued, protected, saved).

// Besides direct negation ("NOT something"), try to avoid words that are not negative per se but carry a negative tone.
// Those words aren't as harmful as direct negation, but they can still make code harder to read. Here's a short list of examples:

// closed → open
// failed → ok, succeeded, successful
// hidden → displayed, shown
// expired → inEffect
// isMissing, isAbsent, isOmitted → exists, provided
// prohibited → allowed

// Positive names make code more intuitive and maintainable, less error-prone, and just nicer to work with.

// Don't forget that here we're talking only about Boolean identifiers.
// Of course, it's absolutely fine to use negative words in the names of non-Boolean database fields and variables (like expiredPoliciesNum), and even in Boolean ones if the negative word is part of the flagged entity.
// For example, in the names "hiddenChargesExist" and "failedBatchJobRegistered", the words "hidden" and "failed" refer to the entities themselves and are acceptable, while the words "exist" and "registered" represent the Boolean aspect of the names.
// Also, this rule doesn't apply to non-Boolean functions that perform negative actions—their names should clearly describe what the function does, such as disableSaveButton, prohibitPrinting, excludeInactiveCustomers, or expirePolicy.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use the NOT operator as little as possible
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Multiple negatives and inverted logic make code harder to analyze and understand.
// Whenever possible, rephrase conditions positively to make your code clearer and more intuitive.
// Use the De Morgan's Laws to simplify Boolean expressions with multiple negations, so the NOT operator is used only once:

(NOT a) OR (NOT b)
// can be rewritten as
NOT (a AND b)

(NOT a) AND (NOT b)
// can be rewritten as
NOT (a OR b)

// A complex expression becomes a simple one written in a positive form ("a AND b" or "a OR b"), which is easier to read and reason about.
// Then, its result is negated, which is even easier - just "NOT TRUE" or "NOT FALSE".

// Example 1 – "AND" becomes "OR":

// The expression
gas = (NOT liquid) AND (NOT solid) AND (NOT plasma)
// should be simplified to
gas = NOT (liquid OR solid OR plasma)

// Example 2 – "OR" becomes "AND":

// The expression
v_stay_home = (NOT i_am_hungry) OR (NOT i_restaurants_are_open_now)
// should be simplified to
v_stay_home = NOT (i_am_hungry AND i_restaurants_are_open_now)

// Now the reader sees a simple positive condition and understands that v_stay_home becomes true only when that condition is not satisfied.

// It's usually best to avoid using the NOT operator in conditional expressions at all whenever possible.
// Many times, I had to maintain existing code that was structured like this:

IF condA AND NOT (condB OR condC) THEN
   proc1;
ELSIF condA AND (condB OR condC) THEN
   proc2;
ELSIF NOT condA AND condD THEN
   proc3;
END IF;

// I was getting a headache just trying to make sense of that mess! You can often reduce that pain by "normalizing" the logic.
// That is similar to how we normalize DB tables structure — by ensuring that repeated pieces (like "condA" and "condB OR condC" in our example) appear only once.
// Now, the logic is much clearer:

IF condA THEN
   IF (condB OR condC) THEN
      proc2;
   ELSE
      proc1;
   END IF;
ELSIF condD THEN
   proc3;
END IF;

// Don't forget to account for the possibility that your expressions might evaluate to NULL. That can lead to unexpected behavior if not handled properly.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use positive comparisons in Boolean expressions
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// *** BAD code: ***

if (orderStatus != OrderStatus.CLOSED) {
   [actions for orders which are not closed]
} else {
   [actions for closed orders]
}

// *** GOOD code: ***

if (orderStatus == OrderStatus.CLOSED) {
   [actions for closed orders]
} else {
   [actions for orders which are not closed]
}

// This advice is especially important if the value comparison is nested into a Boolean expression:

// *** BAD code: ***

if (orderStatus != OrderStatus.CLOSED || state != States.MONTANA || dayType != DayType.WEEKEND) {
   [actions when order is not closed OR state differs from Montana OR day is not a weekend]
} else {
   [actions for other cases]
}

// *** GOOD code: ***

if (orderStatus == OrderStatus.CLOSED && state == States.MONTANA && dayType == DayType.WEEKEND) {
   [actions when order is closed AND state is Montana AND day is a weekend]
} else {
   [actions for other cases]
}

// Anyway, it's better to evaluate the expression in advance, store its result in a Boolean variable with a clear, descriptive name, and use that variable in the if statement — just as described in the "Short conditions in IFs" section.

// The inequality operator is especially confusing when the entire expression is then negated.
// It creates the same kind of "negation of a negation" as in the case of negative names ("if not something is not eqal" - just say "if something is eqal"!).

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use short-circuit evaluation to improve performance
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Short-circuit evaluation (SCE), also called "minimal evaluation", refers to how Boolean operators work in expressions like "A AND B" or "A OR B":
// the second operand is only evaluated if the first operand does not suffice to determine the result of the expression.
// In other words, the evaluation stops as soon as the outcome is known:

A && B
// If A evaluates to false, the whole expression will return false — B is not evaluated.

A || B
// If A evaluates to true, the whole expression will return true — B is not evaluated.

// This means you should structure Boolean expressions so the decision can be made as early as possible.
// Since A will always be executed, make sure that A is more efficient (i.e., quicker to evaluate) than B:

boolean resultX = (localVar1 + localVar2) && this.aBooleanMethodWhichCallsWebService();
boolean resultY = (localVar1 + localVar2) || this.aBooleanMethodWhichCallsWebService();

// If A and B have the same performance cost, prioritize A based on likelihood:

// * In A && B, A should be more likely to evaluate to false
// * In A || B, A should be more likely to evaluate to true

// This increases the chances of skipping the more expensive evaluation of B.

// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// Now let's look at how short-circuit evaluation is used in different programming languages.
// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// It's supported in most C-like syntax languages (C#, JavaScript, TypeScript, Java, Kotlin, Scala) using the standard && and || operators.
// To avoid short-circuit evaluation in these languages, use bitwise operators & and | instead of && and ||.

// In Visual Basic.NET, short-circuit evaluation is supported, but only with specific operators AndAlso (replaces And) and OrElse (replaces Or):
If obj IsNot Nothing AndAlso obj.IsValid() Then ' protect against NullReferenceException
    Console.WriteLine("Condition is true")
End If
If age >= 18 OrElse ExpensiveCheck() Then
    Console.WriteLine("Condition is true")
End If
// To avoid short-circuiting, use the standard And and Or operators.

// In Oracle PL/SQL, short-circuit evaluation is supported, but only with specific operators AND THEN (replaces AND) and OR ELSE (replaces OR):
IF v_coef <> 0 AND THEN (v_var / v_coef) > .2 THEN -- protect against ZERO_DIVIDE
   process(item_id);
END IF;
IF item_in_stock(item_id) OR ELSE is_backorder_allowed(item_id) THEN -- is_backorder_allowed(item_id) will only be called if item_in_stock(item_id) returns FALSE
   approve_order(item_id);
END IF;
// To avoid short-circuiting, use the standard AND and OR operators.

// In T-SQL (Transact-SQL), short-circuit evaluation may occur, but it is not guaranteed or formally defined by the SQL Server and Sybase specification.
// This means that while T-SQL often skips evaluation of the second condition when the result is already determined, developers should not rely on this behavior.
// All expressions should be written assuming that every part will be evaluated, especially when using conditions that involve function calls, subqueries, or potential side effects.

// In PowerScript, the language used in PowerBuilder, short-circuit evaluation is not supported.

// You can easily check whether your programming language supports SCE by assigning a Boolean variable the result of an expression where the second part contains a division by zero.
// Translate the next expression to your language and execute:
boolean sceSuported = ((1 = 2) AND (1 = (1 / 0)))
// If you DON'T get the zero divide error then you are good to go - short-circuiting is there.

// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// How to implement short-circuiting if your language doesn't support short-circuit evaluation
// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// ####### Mimic short-circuit "A AND B": #######

IF A THEN
   IF B THEN
      // do it!
   END IF
END IF

// An alternative way:

boolean doIt = A
IF doIt THEN
   doIt = B
END IF
IF doIt THEN
   // do it!
END IF
// Of course, the name of your boolean var should not be doIt - it must clearly describe the action which will be performed under the overall condition.
// The alternative fragment is longer, but it can result in cleaner code, especially if A and B are more complex expressions than just vars or functions.
// Also, it prevents too much indentation if you have more than two conditions (A, B, C, D...) - you can add any number of "IF doIt THEN" sections, all nicely aligned vertically.

// When returning a boolean value from a function, make its last lines like these:
IF NOT A THEN RETURN FALSE
RETURN B

// ####### Mimic short-circuit "A OR B": #######

boolean doIt = A
IF NOT doIt THEN
   doIt = B
END IF
IF doIt THEN
   // do it!
END IF

// When returning a boolean value from a function, make its last lines like these:

IF A THEN RETURN TRUE
RETURN B
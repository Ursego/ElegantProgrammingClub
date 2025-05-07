// This file contains multiple tips related to Boolean operations.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Short conditions in IFs
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Don't write complex logical expressions directly in if statements.
// Instead, evaluate the expression beforehand, store the result in a Boolean variable with a clear, descriptive name, and use that variable in the if statement.
// This makes the logic behind the condition much easier to understand, as the code becomes self-explanatory.

// *** BAD code: ***

if ((calcMethod == CalcMethods.ADDITIVE && additiveCalculationPassed) || (calcMethod == CalcMethods.RATIO && ratioCalculationPassed))...

// Wow, that's hard to understand! Okay, maybe this example is easy to understand — but think about real-world logical expressions where you can’t make heads or tails of it...
// I’ve seen if statements with half-a-screen-long Boolean expressions!

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
// Of course, this isn’t a critical issue. You can even omit "is" or "are" entirely, and the names will still be clear: rowValidated, ordersOk.

// There’s an exception to this rule — getters of Boolean fields (instance variables). These should follow the standard getField or isField naming convention.
// In Java, this is part of the JavaBean specification, and it makes sense not just for tools (which can infer field names from getters), but also for developers.
// For instance, the method name isInvoicePrinted suggests the existence of a Boolean field invoicePrinted in the class, while invoiceIsPrinted doesn't.

// Never use imperative verbs to name Boolean methods!

// Names like validateRow, checkOrders, or checkFileExistence are completely inappropriate for methods that return Boolean values.
// We don’t say "if validate row then save", "if check orders then print", or "if check file existence then open file".
// I have no explanation for why I have encountered this practice countless times over my decades of working as a programmer.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Use the NOT operator as little as possible
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Double negatives and inverted logic make code harder to analyze and understand.
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

// It’s usually best to avoid using the NOT operator in conditional expressions at all whenever possible.
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

// Don’t forget to account for the possibility that your expressions might evaluate to NULL. That can lead to unexpected behavior if not handled properly.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Try to use positive comparison in Boolean expressions
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// This section, in a way, continues the previous one, since its goal is also to free our lives from negativity. :-)

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
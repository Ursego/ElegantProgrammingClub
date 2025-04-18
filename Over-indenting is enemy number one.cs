// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Indent code fragments using as few tabs as possible.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Avoiding over-indentation forces us to structure our functions properly instead of creating one large, unreadable, and error-prone mess.
// The key word is "forces"—it naturally leads to simpler, more manageable functions. Indentation is like salt in soup: essential, but too much ruins the whole dish.
// If your code has long "staircases" of closing braces (}), END IFs, END LOOPs or other similar branching keywords, it's a sign you should reconsider your coding habits.

// REMARK:
// This article will refer to indentation levels in terms of tab count. When counting tabs, we always start from the base level of left indentation. Here's what that means:
// Some IDEs open a single function in the editor (like PowerBuilder or Oracle Reports), so you begin writing code with no indentation—zero tabs.  
// Most IDEs, however, open an entire class, so the base indentation level is already 2 tabs: to indent the method inside the class, and to indent the code inside the method.
// Still, we will treat this base level (physically 2 tabs) as zero tabs. Let me show you an example using TypeScript:
class FishingReminder {
    checkIfFriday(): void { // the 1st tab is ignored (not counted)
        if (new Date().getDay() === 5) { // the 2nd tab is ignored too - this line has zero indentation and represents the basic level (zero tabs)
            alert("It's Friday, tomorrow you will go fishing!"); // this line has a one tab indentation since we count from the basic level
        }
    }
}
s
// From the book "Clean Code":

// "...the blocks within if statements, else statements, while statements, and so on should be one line long. Probably that line should be a function call.
// Not only does this keep the enclosing function small, but it also adds documentary value because the function called within the block can have a nicely descriptive name.
// This also implies that functions should not be large enough to hold nested structures. Therefore, the indent level of a function should not be greater than one or two.
// This, of course, makes the functions easier to read and understand."

// This quote is probably too strict, but it gives you the idea, the direction. OK, but how many tabs are "too many"?

// 2 tabs are perfectly fine.

// 3 tabs are acceptable if the indented block is short. If it's longer than half a screen, consider extracting it into a separate function.
// That way, your 3 tabs become zero, and you’ll have room to add more indentation later without inviting chaos.
// If the block is nested under several sequential condition checks (each adding a tab), follow the principle described in the topic "Code after validations".

// 4 tabs are a red flag and usually indicate poor code structure. In most cases, the logic can (and should) be written more cleanly.
// As a rare exception, 4 tabs are tolerable if the block is just a few lines long.

// 5 tabs: eternal damnation awaits you. One of my first project managers—a true master of clean code—once said:
// "Do whatever it takes, bend the rules, but never let your code reach 5 tabs of indentation. If I ever see 5 tabs, you’re off the project."

// Note that if you're building a multi-line string, you can format the concatenated sub-strings however you like, regardless of the number of tabs.
// In the next example, 6 tabs are perfectly fine, as they simply show that the tab count doesn’t matter here:

let errorMessageToUser = "First line " +
                         "Second line " +
                         "Third line "

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Fragments that logically belong to the same level of nesting should be indented with the same number of tabs, whenever possible.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If you follow the proper formatting rule, the structure of your code is immediately clear to the reader. Indentation should clarify, not confuse.

// Here’s an example of a confusing structure commonly found in many applications.
// Logically, the code blocks are on the same nesting level (since only one of them will eventually run), but they are indented with different numbers of tabs.
// This makes the structure harder to understand.
// The example is simple, but in real-world code, the number of conditions can be much greater, and each block may be long, with its own indentation levels.

if <condition 1> then
    [code fragment 1]
else
    if <condition 2> then
        [code fragment 2]
    else
        if <condition 3> then
            [code fragment 3]
        else
            [code fragment 4]
        end if
    end if
end if

// The indentation, while clearly consistent, doesn’t help.
// By removing unnecessary grouping and indenting to reflect that the block is essentially a single if structure, everything becomes much clearer.
// Now the fragments that are logically on the same level of nesting also share the same indentation level:

if <condition 1> then
    [code fragment 1]
elseif <condition 2> then
    [code fragment 2]
elseif <condition 3> then
    [code fragment 3]
else
    [code fragment 4]
end if

// Now it's clear that one, and only one, case will be executed. Reading from top to bottom until the matching condition is found shows exactly which one.

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// In loops, use `continue` instead of adding another level of indentation to the remaining code.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// This method is a powerful tool in the fight against over-indentation.

// *** BAD code: ***

while ([loop condition]) {
    if ([condition 1])  {
        if ([condition 2]) {
            if [condition 3]) {
                [code fragment with its own indenting levels]
            }
        }
    }
}

// *** GOOD code: ***

while ([loop condition]) {
    if (![condition 1]) continue;
    if (![condition 2]) continue;
    if (![condition 3]) continue;
    [code fragment with its own indenting levels]
}

// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Code after validations
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// If a LARGE code block is executed after a series of validations (and is nested inside multiple 'if' statements), refactor both the validations and the block into a new function.
// Then, exit the function immediately after the first failed validation:

// *** BAD code: ***

private void method1 () {
    [code fragment A]

    if (this.dataOk1()) {
        if (this.dataOk2()) {
            if (this.dataOk3()) {
                [code fragment with its own indents]
            }
    }

    [code fragment B]
}

// *** GOOD code: ***

private void method2 () {
   if (!this.dataOk1()) return;
   if (!this.dataOk2()) return;
   if (!this.dataOk3()) return;

   [code fragment with its own indents]
}

private void method1 () {
   [code fragment A]

   this.method2();

   [code fragment B]
}

// This not only reduces unnecessary indentation, but also clearly communicates the intent:
// The entire algorithm is executed only if all preliminary validations pass.
// The validations are not part of the algorithm—they simply determine whether the logic should run at all.

// You might ask: what about the "single point of exit" rule? I won't go deep into that here, but in my view, this idea causes more problems than it solves.
// I agree with Edsger W. Dijkstra, who was strongly against the single exit concept.
// It might help with debugging in some cases, but why should I deal with more complex code every day just to make future debugging—if it even happens—a bit easier?

// What if a code block runs after several validations, like in the previous example, but you don’t want to refactor it into a new function since it’s short and doesn’t deserve one?
// Just use a Boolean flag to control the logic locally:

// *** Another GOOD code: ***

private void method1 () {
    [code fragment A]

    Boolean ok = this.dataOk1();
    if (ok) ok = this.dataOk2();
    if (ok) ok = this.dataOk3();
    if (ok) {
        [code fragment with its own indents]
    }

    [code fragment B]
}
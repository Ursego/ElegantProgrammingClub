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
// If you often query data from a single database table using different search criteria, create one generic stored procedure.  
// Pass the optional search criteria through optional (i.e. with default values) parameters.
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  
// Then, call this procedure from various places, passing custom parameter sets as needed.
// Obviously, the WHERE clause should restrict only by the parameters through which the values ​​are actually passed.

// Now I'll show you a real Sybase T-SQL procedure I wrote while working at an insurance company.
// Requests for the number of client claims are very common in the system—for example, to determine eligibility for certain coverages or to calculate premiums.  
// While the eligibility criteria and calculation rules vary widely, the core logic stays the same, and the search is always performed across two tables.
// The total length of duplicated code through the system was shocking! I couldn’t stand it any longer, and wrote this procedure:

CREATE PROCEDURE s_get_clm_cnt
   @pol_no             numeric(12),
   @pol_ver_dt         smalldatetime,
   @prior_to_dt        smalldatetime,   -- usually pass pol_term_eff_dt, but not always - depending on business requirements
   @period_in_years    smallint,        -- for how much time before @prior_to_dt
   @at_fault           char(1),         -- 'Y' = AF only, 'N' = not-AF only, 'A' = Any (AF or not-AF)
   @before_period      char(1) = 'N',   -- 'Y' = BEFORE @period_in_years, 'N' = DURING @period_in_years; usually 'N', so you can omit this arg in most cases
   @pol_ver_planloc_no smallint = NULL, -- vehicle; omit to count all vehicles on policy
   @parm__pol_drv_no   smallint = NULL, -- omit if you don't need to restrict by it (i.e. if you need to count all drivers on vehicle)
   @parm__pol_clm_typ  tinyint  = NULL, -- omit if you don't need to restrict by it.
                                        -- The arg is ignored if @at_fault = 'Y'. In thes case, the filter is: pol_clm_typ = 100 /* Chargeable */
   @parm__plan_cd      smallint = NULL, -- omit if you don't need to restrict by it
   @clm_cnt            smallint OUTPUT
AS
/******************************************************************************************************************************************
Returns the total count of claims for given vehicle for given period of time per given parameters.
*******************************************************************************************************************************************
Michael Zuskin 29-Oct-2019  Created for Portal UW Rules
*******************************************************************************************************************************************/

DECLARE
   @nm_no           int,
   @gis_clm_cnt     smallint,
   @non_gis_clm_cnt smallint

IF @parm__pol_drv_no IS NOT NULL BEGIN
   SELECT @nm_no = nm_no
     FROM pol_drv
    WHERE pol_no     = @pol_no
      AND pol_ver_dt = @pol_ver_dt
      AND pol_drv_no = @parm__pol_drv_no
END

SELECT @gis_clm_cnt = count(*)
  FROM pol_ver_clm c
      ,pol_ver v
 WHERE c.pol_no             = @pol_no
   AND c.pol_ver_dt         = @pol_ver_dt
   AND (c.charge_planloc_no = @pol_ver_planloc_no OR @pol_ver_planloc_no IS NULL)
   AND v.pol_no             = c.pol_no
   AND v.pol_ver_dt         = c.pol_ver_dt
   
   AND sa_clm_appl_cd      IN (1 /* Overriden */, 4 /* Automatic */)
   AND sa_clm_no           IS NULL -- Ignore Related Claims

   AND (c.drv_nm_no   = @nm_no             OR @parm__pol_drv_no  IS NULL) 
   AND (c.pol_clm_typ = @parm__pol_clm_typ OR @parm__pol_clm_typ IS NULL)
   AND (c.plan_cd     = @parm__plan_cd     OR @parm__plan_cd     IS NULL)

   AND (
          (@before_period = 'Y' AND c.loss_dt <  DateAdd (year, (@period_in_years * -1), @prior_to_dt))
          OR
          (@before_period = 'N' AND c.loss_dt >= DateAdd (year, (@period_in_years * -1), @prior_to_dt))
       )

   AND (
          (@at_fault IN ('Y', 'A') AND c.pol_clm_typ =  100 /* Chargeable */)
          OR
          (@at_fault IN ('N', 'A') AND c.pol_clm_typ <> 100 /* Chargeable */)
       )

SELECT @non_gis_clm_cnt = count(*)
  FROM pol_ver_oth_clm c, 
       nm_drv_oth_clm_ver v,
       pol_ver pv
 WHERE c.pol_no                = @pol_no
   AND c.pol_ver_dt            = @pol_ver_dt
   AND (c.charge_planloc_no    = @pol_ver_planloc_no OR @pol_ver_planloc_no IS NULL)
   AND pv.pol_no               = c.pol_no
   AND pv.pol_ver_dt           = c.pol_ver_dt
   AND c.nm_no                 = v.nm_no
   AND c.nm_drv_oth_clm_no     = v.nm_drv_oth_clm_no
   AND c.nm_drv_oth_clm_ver_dt = v.nm_drv_oth_clm_ver_dt

   AND sa_clm_appl_cd          IN (1 /* Overriden */, 4 /* Automatic */)
   AND v.oth_clm_amt           >= 0                              
   AND isnull(c.chg_sta_cd,'N') <> 'D'

   AND ((v.nm_no = @nm_no OR v.drv_nm_no = @nm_no) OR @parm__pol_drv_no  IS NULL)
   AND (v.pol_clm_typ = @parm__pol_clm_typ         OR @parm__pol_clm_typ IS NULL)
   AND (v.plan_cd     = @parm__plan_cd             OR @parm__plan_cd     IS NULL)

   AND (
          (@before_period = 'Y' AND v.loss_dt <  DateAdd (year, (@period_in_years * -1), @prior_to_dt))
          OR
          (@before_period = 'N' AND v.loss_dt >= DateAdd (year, (@period_in_years * -1), @prior_to_dt))
       )

   AND (
          (@at_fault IN ('Y', 'A') AND v.pol_clm_typ =  100 /* Chargeable */)
          OR
          (@at_fault IN ('N', 'A') AND v.pol_clm_typ <> 100 /* Chargeable */)
       )

SET @clm_cnt = IsNull(@gis_clm_cnt, 0) + IsNull(@non_gis_clm_cnt, 0)

RETURN

// The reaction of some colleagues who had been on the project for years was interesting.  
// Some looked at me like a super-programmer — even though writing this procedure (and a few others like it) was far easier than launching a rocket to Mars.  
// And one developer said these procedures shouldn’t be used—for the sake of consistency with the old code.
// But I have no interest in being consistent with bad practices!

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

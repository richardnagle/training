## TDD Schools ##
* Use the term *mock* to cover all types of doubles - stubs, mocks, fakes, spyies, etc.

## Mockist Outside-In ##

### moi1 Create Acceptance Test ###

* Acceptance tests are mandatory in GOOS style
* Walking skeleton
* Show test
* Show `ServiceRunner`
    * Explain the `empty.http` file and how its loaded as embedded resource
    * Show how the headers and body will be passed on the command line
    * Briefly go over `ExternalProgram`
* Show `Database`
    * Explain our simple persistance strategy
    * Show how the asserts work

### moi2 Walking Skeleton ###

* Show `Handler`
* Show `Program`
* Emphasis that this is bear minimum to get working
* No unit tests yet
* Show that test passes

### moi3 First Unit Test ###

* Show new acceptance test and valid.http file
* Show new `ReviewHandlerTests` fixture
    * Explain the test
    * Talk about why the observer
        * Tell don't ask - void methods avoid stubs
        * Interested in the interaction not outputs
    * Note the use of literals
* Show `ReviewHandler`, `Headers` and `FormattedReview`
* Show `RequestRepository`
    * Explain we needed this to keep 1st acceptance test running
* Note that one test has spawned several classes
* Note that new acceptance tests fails

### moi4 Request Repository ###

* Show `ReviewRepositoryTests`
* Show `ReviewRepository` & `DatabaseWriter`
* All tests pass

### moi5 Add html formatting ###

* Show that we extended the acceptance test
* Show `ReviewHandlerTests`
    * Notice that we have split the test by parameter passed
    * Show the four new tests for html formatting
    * Note that we are still testing through the handler - we are not creating a separate test for html formatting
* Show `FormattedReview`
    * Notice the denseness of code - we need to refactor
* Reflect that we added a test to `ReviewHandlerTests` but didn't change `ReviewHandler`
* All tests pass

### moi6.1 / moi6.2 Refactoring html formatting ###

* 6.1 reflect that when you extract a method as a static often means you can extract class
* We are free to refactor without changing tests
* All the tests pass

### moi7 Add the validation

* Explain strategy
    * http return code as exit code
    * `'Review created'` or error message shown on console
* Show new acceptance test and .http files
    * Look at how asserts are implemented
* Show the validation tests in `ReviewHandlerTests`
* Show the new notification test in `ReviewRepositoryTests`
* Show the implementation
    * Value objects notify when the validation fails
    * `ReviewHandler` will not save if validation saves
    * `Main.RequestProcessor` reports success or failure
* Note the decoupled design and SRP
* Show the spare method in `ReviewHandler`
* All the test pass

### moi8 One final refactoring
* Split the interface

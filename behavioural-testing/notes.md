## TDD Schools ##
* Use the term *mock* to cover all types of doubles - stubs, mocks, fakes, spies, etc.

## Mockist Inside-Out ##

### Show Framework Code
`git checkout code-start-point`
* Introduce solution structure
* Show how handlers work
    * `IHandle<T>`
    * `Request`
    * `PostedReview`
    * `Response`
* Show persistance
    * `ISaveReviews`
    * `ReviewDto`

### Create ReviewValidator
`git checkout ??tag??`
* Show test
    * Mention that we wont use `[SetUp]` as it makes test easier to read on a course
    * Explain lack of mocks - we're at bottom level at the moment
* Show `ReviewValidator`
* Show we create an interface `IReviewValidator`

### Create SectionWalker
`move-git-next`
* Same pattern as before

### Create ReviewDtoMapper
`move-git-next`
* Same pattern as before

### Create ReviewHtmlFormatter
`move-git-next`
* Same patten as before
* Asks what people feel about the classes we created
    * Lack cohesion
    * Procedural code
    * -er classes

### Integrate validation and saving into handler
`move-git-next`
* Show handler tests    
* Show handler

### Integrate remaining components into handler
`move-git-next`
* look at `Saved_review_has_sections_formatted_as_html` test
    * Talk about test complexity
        * Look at all the set-up
        * Need to set-up things that are inconsequential to the test (validation)
* Show handler
    * Talk about implementation complexity
        * Constructor
        * SRP
        * Conditional complexity
* Maybe test complexity leads to implementation complexity

### Refactoring - move SectionWalker inside HtmlFormatter
* show that in Handler the `SectionWalker` produces data for `HtmlFormatter` so the dependency can be moved in there.
`move-git-next`
* Show changes to `ReviewHtmlFormatter` and test

* Then demonstrate how hard the refactoring is in Handler
    * Use auto-refactoring tools to remove the `SectionWalker` dependency
    * Need to change the test in the middle of the refactoring

## Mockist Outside-In ##

### Create Acceptance Test ###
`git checkout ??tag??`
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

### Walking Skeleton ###
`move-git-next`
* Show `Handler`
* Show `Program`
* Emphasis that this is bear minimum to get working
* No unit tests yet
* Show that test passes

### First Unit Test ###
`move-git-next`
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

### Request Repository ###
`move-git-next`
* Show `ReviewRepositoryTests`
* Show `ReviewRepository` & `DatabaseWriter`
* All tests pass

### Add html formatting ###
`move-git-next`
* Show that we extended the acceptance test
* Show `ReviewHandlerTests`
    * Notice that we have split the test by parameter passed
    * Show the four new tests for html formatting
    * Note that we are still testing through the handler - we are not creating a separate test for html formatting
* Show `FormattedReview`
    * Notice the denseness of code - we need to refactor
* Reflect that we added a test to `ReviewHandlerTests` but didn't change `ReviewHandler`
* All tests pass

### Refactoring html formatting ###
`move-git-next`
* 6.1 reflect that when you extract a method as a static often means you can extract class
* We are free to refactor without changing tests
* All the tests pass

### Add the validation
`move-git-next`
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

### One final refactoring
`move-git-next`
* Split the interface

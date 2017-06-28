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

## Classicist Inside-Out ##

### Add Components ###
`git checkout ??tag??`
* Same components and tests
* Interfaces have been removed

### First unit test for ReviewHandler ###
`move-git-next`
* Basic start-up test

### Tests for saving ###
`move-git-next`
* Show test
    * Note how we are passing real dependencies in constructor
    * Talk about how we only test enough to get the integration
        * Didn't test Author - same source as ISBN
        * Use of regex for body
    * Note the use of a mock - explain why (external dependency)
> Show Slide - But, Hang-On, you used a Mock

### Creating a Builder  ###
* Show complexity in existing test
`move-git-next`
* Show builder

> Show Slide - Creating Test Builders

* Show cleaned-up test
    * In 1st test we just use default values
    * In 2nd test we assign values pertinent to that test - never rely or assert on default builder values

### Add Validation to ReviewHandler  ###
`move-git-next`
* Show tests and implementation
* Point out how builder enables quick and clean tests

### Refactoring - move SectionWalker inside HtmlFormatter
* Add this test to `ReviewHtmlFormatterTest`
```
        [Test]
        public void Formats_text_to_html2()
        {
            var sections = new[]
            {
                new ReviewSection {Name = "title", Text = "the title"},
                new ReviewSection {Name = "subtitle", Text = "the sub title"},
                new ReviewSection {Name = "body", Text = "the content"},
            };

            var htmlFormatter = new ReviewHtmlFormatter(new SectionWalker());

            var formatted = htmlFormatter.Format(sections);
            Assert.That(formatted, Is.EqualTo("<h1>the title</h1>\r\n<h2>the sub title</h2>\r\n<p>the content</p>\r\n"));
        }
```
* Implement without removing existing functionality
* Change each `ReviewHtmlFormatter`constructor in `ReviewHandlerTests`
* Change `ReviewHandler`
* Tidy up
    * Unused code in `ReviewHandler`
    * Unused `SectionWalker` in `ReviewHandler`
    * Remove redundant test in `ReviewHtmlFormatterTests`
    * Remove ctor, make old `Format` private and rename


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

## Classicist Outside-in

### Create first unit test
`git checkout ??tag??`
* Show test
    * Note use of builder copied from previous example
    * Note lack of dependencies - we're going straight into handler
* Show implementation

### Add validation
`move-git-next`
* Show how we add negative and positive cases to demonstrate behavior

### Refactor - add HttpHeader
* Show duplication in accessing headers
`move-git-next`
* Show HttpHeaders
* Show ReviewHeaders
* Note refactoring was possible with test changes

### Refactor - create value objects
* Show duplication is structure - three `if` statements returning `new Response`
`move-git-next`
* Show ContentType class
* Show how its constructed in HttpHeaders
* Show usage in Handler
`move-git-next`
* Show Referer and Isbn classes repeating the prev pattern
* Show Handler - still duping `if` but now we can see the abstraction
`move-git-next`
* Show IValidateAReview
* It by implemented in Isbn, ContentType and Referer
* Show how it cleans up Handler
* Re-emphasis - all this refactoring was done without test changes

### Adding save
`move-git-next`
* Show test - note the use of a mock again
* Show implementation - again the code has just been jammed in, to be refactored next
`move-git-next`
* Show `Isbn` and `Refer` to see `Populate()` method in each
`move-git-next`
* Show `HtmlBody`
* Show `Section`
* Show `Review`
* Note both have `Populate()` methods
`move-git-next`
* Show `IPopulateReviewDto` and implemented in `Review`, `Isbn`, `HtmlBody`, `Referer`
* Show implementation in `Handler`
`move-git-next`
* Show `Review`
* Show cleaned-up `Handler`
* Talk about levels of abstraction
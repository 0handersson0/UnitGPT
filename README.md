
<img src="https://github.com/0handersson0/UnitGPT/assets/72985598/ace7ec7a-efa3-4714-b4da-ac615354e729" alt="drawing" width="100"/>

# UnitGPT
<a href="https://github.com/0handersson0/UnitGPT/blob/master/UnitGPT/LatestRelease/UnitGPT.vsix">Latest release</a>
<td class="ux-itemdetails-left"><div class="itemDetails"><div class="markdown"><p>UnitGPT is a small extension that will help with creating code and unit tests with the power of openAI.</p>
  
  ![CreateCodeDemo](https://github.com/0handersson0/UnitGPT/assets/72985598/b72decfe-e6e1-4772-9c56-d4eeceb58eb1)

<h2 id="generate-code">Generate code</h2>
<p>To create a code suggestion simply write: <strong>// ### instructions here  ###</strong>  any where in the active document and press <strong>Alt+C</strong> or the menu option available under Edit.</p>
<p><img src="https://upnortbytes.gallerycdn.vsassets.io/extensions/upnortbytes/unitgpt/1.0.1/1684914472198/image.png" alt="image.png"></p>
<p><img src="https://github.com/0handersson0/UnitGPT/assets/72985598/567d761b-abfa-4c63-a226-a9f4a15003e1" alt="image__1.png"></p>
<p>The generated code will be inserted under the instructions:</p>
<p><img src="https://upnortbytes.gallerycdn.vsassets.io/extensions/upnortbytes/unitgpt/1.0.1/1684914472198/image__2.png" alt="image__2.png"></p>
<h2 id="unit-test">Unit test</h2>
<p>Make sure the test project is referenced under the setting for the extension and that the correct framework for the testproject is selected</p>
<p><img src="https://github.com/0handersson0/UnitGPT/assets/72985598/fcc4006f-8410-4fb8-ab09-32917e33d736" alt="image__4.png"></p>
<p>Highlight the code you want to create unit test for and press <strong>Alt+U</strong> or the menu option available under /Edit and a new test file will be created in the selected xUnit project.</p>
<p><img src="https://github.com/0handersson0/UnitGPT/assets/72985598/567d761b-abfa-4c63-a226-a9f4a15003e1" alt="image__7.png"></p>
<p><img src="https://upnortbytes.gallerycdn.vsassets.io/extensions/upnortbytes/unitgpt/1.0.1/1684914472198/image__6.png" alt="image__6.png"></p>
<p><img src="https://upnortbytes.gallerycdn.vsassets.io/extensions/upnortbytes/unitgpt/1.0.1/1684914472198/image__5.png" alt="image__5.png"></p>
<h2 id="options">Options</h2>
<p>The extension has three option.</p>
<ol>
<li>The key that is required to make the openAi api calls. You will need to register on <a href="https://platform.openai.com/" target="_blank" rel="noreferrer noopener nofollow">https://platform.openai.com/account/api-keys</a> and create your own key.</li>
<li>The test project where you want to place you're generated unit tests.</li>
<li>The type of the test generated.</li>
</ol>
<p><img src="https://github.com/0handersson0/UnitGPT/assets/72985598/fcc4006f-8410-4fb8-ab09-32917e33d736" alt="image__4.png"></p>
</div></div></td>

<h2>Troubleshooting Guide for UnitGPT</h2>

Problem: Code suggestion is not generated.

Solution:
1. Check that the UnitGPT extension is installed and enabled in your Visual Studio.
2. Ensure that you have included the following comment anywhere in the active document: `// ### instructions here ###`.
3. Make sure you are pressing Alt+C or using the correct menu option under Edit to trigger the code generation.
4. Verify that you have a stable internet connection to access the OpenAI API.
5. If the issue persists, try restarting Visual Studio and attempting the code generation again.

Problem: Generated code is not inserted as expected.

Solution:
1. Ensure that the `// ### instructions here ###` comment is correctly placed in the active document.
2. Verify that you are pressing Alt+C or using the menu option under Edit after adding the comment.
3. Check if there are any syntax errors or conflicts in the code that prevent the generated code from being inserted.
4. If the issue persists, try closing and reopening the document or restarting Visual Studio.

Problem: Unable to create unit tests.

Solution:
1. Confirm that the test project is referenced correctly in the settings of the UnitGPT extension.
2. Check that the appropriate framework for the test project is selected. Ensure compatibility between the framework and the test project.
3. Highlight the specific code for which you want to create a unit test.
4. Press Alt+U or use the menu option under Edit to initiate the unit test generation.
5. Make sure you have the necessary permissions to create files in the selected xUnit project.
6. If the test file is not created, check for any error messages or notifications that may provide additional information.

Problem: API Key is not working.

Solution:
1. Ensure that you have registered on https://platform.openai.com/account/api-keys and obtained your own API Key.
2. Double-check that the API Key is correctly entered in the UnitGPT extension settings.
3. Confirm that the API Key has not expired or been revoked by OpenAI.
4. Verify that your internet connection is stable and allows communication with the OpenAI API.
5. If the problem persists, try generating a new API Key and updating it in the extension settings.

Problem: Unable to specify the test project or type of test generated.

Solution:
1. Check the extension's settings to ensure that the test project option is available and properly configured.
2. Verify that you have the necessary permissions to modify the test project and create unit tests.
3. If the test project is not selectable or editable, confirm that it meets the requirements of the UnitGPT extension.
4. Ensure that the test type option is accessible and choose the desired type for the generated tests.

If you encounter any persistent issues or errors that cannot be resolved using this troubleshooting guide, consider reaching out to the UnitGPT extension support team for further assistance.

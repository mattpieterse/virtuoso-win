#!/usr/bin/env node


import {
    readFileSync,
    appendFileSync
} from 'fs';


// --- Script


try {
    const githubRef = process.env.GITHUB_REF;
    const headerRef = process.env.GITHUB_HEAD_REF;

    const branch = headerRef || (githubRef || '').replace(/^refs\/heads\//, '');

    if (githubRef && githubRef.startsWith('refs/tags/')) {
        console.log(`ℹ️ Skipping branch validation for tag refs: ${githubRef}`);
        process.exit(0);
    }

    console.log(`🔍 Validating branch: ${branch}`);

    const regex = /^(main|staging|topic\/[a-z0-9-]+|debug\/[a-z0-9-]+|tests\/[a-z0-9-]+|rollback\/[a-z0-9-]+)$/;

    if (regex.test(branch)) {
        console.log('✅ Follows accepted conventions.');
        process.exit(0);
    } else {
        console.log(`❌ Invalid branch name.`);
        console.log('See summary for details.');

        let summary = readFileSync(new URL('./action.md', import.meta.url), 'utf8')
            .replace('{{BRANCH_NAME}}', branch);

        appendFileSync(process.env.GITHUB_STEP_SUMMARY, summary);
        process.exit(1);
    }
} catch (error) {
    console.error(error.message);
    process.exit(1);
}

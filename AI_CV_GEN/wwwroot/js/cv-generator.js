// ===== CV Generator AI - JavaScript =====
// Matches CV_Generator.ViewModels.CVFormSubmit exactly

let experienceEntries = [];
let projectEntries = [];
let skillTags = [];

// ══════════════════════════════════════════
//  SKILLS (List<string>)
// ══════════════════════════════════════════

function addSkillTag() {
    const input = document.getElementById('skillInput');
    const val = input.value.trim();
    if (!val) return;

    skillTags.push(val);
    renderSkillTags();
    input.value = '';
    input.focus();
}

function removeSkillTag(index) {
    skillTags.splice(index, 1);
    renderSkillTags();
}

function renderSkillTags() {
    const container = document.getElementById('skillTagsContainer');
    container.innerHTML = '';
    skillTags.forEach((skill, i) => {
        const tag = document.createElement('span');
        tag.className = 'skill-tag';
        tag.innerHTML = `${skill} <button type="button" onclick="removeSkillTag(${i})">×</button>`;
        container.appendChild(tag);
    });
}

// Allow pressing Enter to add a skill
document.addEventListener('DOMContentLoaded', function () {
    const skillInput = document.getElementById('skillInput');
    if (skillInput) {
        skillInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') { e.preventDefault(); addSkillTag(); }
        });
    }

    // Close modal on overlay click
    const modal = document.getElementById('resultModal');
    if (modal) {
        modal.addEventListener('click', function (e) {
            if (e.target === this) closeModal();
        });
    }
});

// ══════════════════════════════════════════
//  EXPERIENCE (List<Experience>)
//  Fields: Role, Company, StartDate, EndDate, Description
// ══════════════════════════════════════════

function addExperience() {
    const id = Date.now();
    experienceEntries.push(id);
    const list = document.getElementById('experienceList');
    const block = document.createElement('div');
    block.className = 'entry-block';
    block.id = 'exp_' + id;
    block.innerHTML = `
        <button type="button" class="remove-btn"
                onclick="removeEntry('exp_${id}', experienceEntries, ${id}, 'deleteExpBtn')">
            <i class="fa-solid fa-xmark"></i>
        </button>
        <div class="labeled-input">
            <span class="label-tag">Role</span>
            <input type="text" data-field="role" placeholder="e.g. Software Engineer" />
        </div>
        <div class="labeled-input">
            <span class="label-tag">Company</span>
            <input type="text" data-field="company" placeholder="e.g. Tech Corp" />
        </div>
        <div class="two-date-row">
            <div class="labeled-input">
                <span class="label-tag">Start Date</span>
                <input type="date" data-field="startDate" />
            </div>
            <div class="labeled-input">
                <span class="label-tag">End Date</span>
                <input type="date" data-field="endDate" />
            </div>
        </div>
        <div class="labeled-input">
            <span class="label-tag">Description</span>
            <input type="text" data-field="description" placeholder="Key responsibilities..." />
        </div>
    `;
    list.appendChild(block);
    updateDeleteBtn('deleteExpBtn', experienceEntries);
}

function deleteLastExperience() {
    if (experienceEntries.length === 0) return;
    const lastId = experienceEntries[experienceEntries.length - 1];
    animateRemove('exp_' + lastId, () => {
        experienceEntries.pop();
        updateDeleteBtn('deleteExpBtn', experienceEntries);
    });
    showToast('Last experience entry removed');
}

// ══════════════════════════════════════════
//  PROJECTS (List<Project>)
//  Fields: Name, Technologies, Description, GitHubLink
// ══════════════════════════════════════════

function addProject() {
    const id = Date.now();
    projectEntries.push(id);
    const list = document.getElementById('projectList');
    const block = document.createElement('div');
    block.className = 'entry-block';
    block.id = 'proj_' + id;
    block.innerHTML = `
        <button type="button" class="remove-btn"
                onclick="removeEntry('proj_${id}', projectEntries, ${id}, 'deleteProjBtn')">
            <i class="fa-solid fa-xmark"></i>
        </button>
        <div class="labeled-input">
            <span class="label-tag">Name</span>
            <input type="text" data-field="name" placeholder="e.g. E-commerce API" />
        </div>
        <div class="labeled-input">
            <span class="label-tag">Technologies</span>
            <input type="text" data-field="technologies" placeholder="e.g. C#, ASP.NET Core, SQL Server" />
        </div>
        <div class="labeled-input">
            <span class="label-tag">Description</span>
            <input type="text" data-field="description" placeholder="What does it do?" />
        </div>
        <div class="labeled-input">
            <span class="label-tag">GitHub Link</span>
            <input type="url" data-field="gitHubLink" placeholder="https://github.com/..." />
        </div>
    `;
    list.appendChild(block);
    updateDeleteBtn('deleteProjBtn', projectEntries);
}

function deleteLastProject() {
    if (projectEntries.length === 0) return;
    const lastId = projectEntries[projectEntries.length - 1];
    animateRemove('proj_' + lastId, () => {
        projectEntries.pop();
        updateDeleteBtn('deleteProjBtn', projectEntries);
    });
    showToast('Last project entry removed');
}

// ══════════════════════════════════════════
//  SHARED HELPERS
// ══════════════════════════════════════════

function updateDeleteBtn(btnId, arr) {
    const btn = document.getElementById(btnId);
    if (btn) btn.disabled = arr.length === 0;
}

function removeEntry(elementId, array, id, deleteBtnId) {
    animateRemove(elementId, () => {
        const idx = array.indexOf(id);
        if (idx > -1) array.splice(idx, 1);
        if (deleteBtnId) updateDeleteBtn(deleteBtnId, array);
    });
}

function animateRemove(elementId, callback) {
    const el = document.getElementById(elementId);
    if (el) {
        el.style.transition = 'opacity 0.2s, transform 0.2s';
        el.style.opacity = '0';
        el.style.transform = 'translateY(-8px)';
        setTimeout(() => { el.remove(); if (callback) callback(); }, 200);
    } else {
        if (callback) callback();
    }
}

// ══════════════════════════════════════════
//  PREPARE SUBMIT
//  Serializes all dynamic data into hidden
//  JSON fields before the form posts to the server.
//  Maps exactly to CVFormSubmit fields.
// ══════════════════════════════════════════

function prepareSubmit() {
    // ── Skills → List<string> ──
    document.getElementById('skillsJson').value = JSON.stringify(skillTags);

    // ── Experience → List<Experience> ──
    const experiences = [];
    document.querySelectorAll('#experienceList .entry-block').forEach(block => {
        experiences.push({
            role:        block.querySelector('[data-field="role"]')?.value?.trim()        || '',
            company:     block.querySelector('[data-field="company"]')?.value?.trim()     || '',
            startDate:   block.querySelector('[data-field="startDate"]')?.value           || '',
            endDate:     block.querySelector('[data-field="endDate"]')?.value             || '',
            description: block.querySelector('[data-field="description"]')?.value?.trim() || ''
        });
    });
    document.getElementById('experienceJson').value = JSON.stringify(experiences);

    // ── Projects → List<Project> ──
    const projects = [];
    document.querySelectorAll('#projectList .entry-block').forEach(block => {
        projects.push({
            name:         block.querySelector('[data-field="name"]')?.value?.trim()         || '',
            technologies: block.querySelector('[data-field="technologies"]')?.value?.trim() || '',
            description:  block.querySelector('[data-field="description"]')?.value?.trim()  || '',
            gitHubLink:   block.querySelector('[data-field="gitHubLink"]')?.value?.trim()   || ''
        });
    });
    document.getElementById('projectsJson').value = JSON.stringify(projects);

    // ── Button loading state ──
    const btn = document.getElementById('generateBtn');
    btn.classList.add('loading');
    btn.innerHTML = '<i class="fa-solid fa-circle-notch fa-spin"></i> Generating...';
}

// ══════════════════════════════════════════
//  MODAL
// ══════════════════════════════════════════

function closeModal() {
    document.getElementById('resultModal').classList.remove('active');
}

function downloadCV() {
    const text = document.getElementById('cvPreview').textContent;
    const blob = new Blob([text], { type: 'text/plain' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = 'my_cv.txt';
    a.click();
    showToast('CV downloaded successfully! 🎉');
}

function copyCV() {
    const text = document.getElementById('cvPreview').textContent;
    navigator.clipboard.writeText(text).then(() => {
        showToast('CV copied to clipboard! ✓');
    });
}

// ══════════════════════════════════════════
//  TOAST
// ══════════════════════════════════════════

function showToast(msg) {
    const t = document.getElementById('toast');
    t.textContent = msg;
    t.classList.add('show');
    setTimeout(() => t.classList.remove('show'), 2800);
}

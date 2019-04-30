<template>
  <nav>
    <v-toolbar app flat>
      <v-toolbar-side-icon @click="drawer = !drawer"></v-toolbar-side-icon>
      <v-toolbar-title>Consilium</v-toolbar-title>
    </v-toolbar>

    <v-navigation-drawer app v-model='drawer'>
      <v-list>

        <!--//* Avatar -->
        <v-list-tile avatar>
          <v-list-tile-avatar>
            <img src="https://randomuser.me/api/portraits/men/85.jpg">
          </v-list-tile-avatar>
          <v-list-tile-content>
            <v-list-tile-title>Marvin Zeising</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>

        <!--//* Home -->
        <v-list-tile :to="{ name: 'home' }">
          <v-list-tile-action>
            <v-icon>home</v-icon>
          </v-list-tile-action>
          <v-list-tile-title>Home</v-list-tile-title>
        </v-list-tile>

        <!--//* Project loader -->
        <v-list-tile v-if="loadingProjects">
          <v-list-tile-action>
            <v-progress-circular
              indeterminate
              color="primary"
            />
          </v-list-tile-action>
          <v-list-tile-title>Loading Projects...</v-list-tile-title>
        </v-list-tile>

        <!--//* Projects -->
        <v-list-group
          v-for="(project, i) in myProjects"
          :key="i"
          prepend-icon="extension"
          value="true"
        >
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title v-text="project.name"></v-list-tile-title>
            </v-list-tile>
          </template>

          <v-list-tile
            v-for="(action, j) in project.actions"
            :key="j"
            :to="{ name: action[2], params: { projectId: project.id }}"
          >
            <v-list-tile-action>
              <v-icon v-text="action[1]"></v-icon>
            </v-list-tile-action>
            <v-list-tile-title v-text="action[0]"></v-list-tile-title>
          </v-list-tile>

          <v-list-group
            no-action
            sub-group
          >
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-title>Administration</v-list-tile-title>
              </v-list-tile>
            </template>

            <v-list-tile
              v-for="(action, j) in project.adminActions"
              :key="j"
              :to="{ name: action[2], params: { projectId: project.id }}"
            >
              <v-list-tile-action>
                <v-icon v-text="action[1]"></v-icon>
              </v-list-tile-action>
              <v-list-tile-title v-text="action[0]"></v-list-tile-title>
            </v-list-tile>
          </v-list-group>

        </v-list-group>

        <!--//* Profile -->
        <v-list-group
          prepend-icon="person"
        >
          <template v-slot:activator>
            <v-list-tile>
              <v-list-tile-title>Profile</v-list-tile-title>
            </v-list-tile>
          </template>

          <v-list-tile
            v-for="(action, i) in profileActions"
            :key="i"
            :to="{ name: action[2] }"
          >
            <v-list-tile-action>
              <v-icon v-text="action[1]"></v-icon>
            </v-list-tile-action>
            <v-list-tile-title v-text="action[0]"></v-list-tile-title>
          </v-list-tile>
        </v-list-group>

        <!--//* Sign out -->
        <v-list-tile to="signUp">
          <v-list-tile-action>
            <v-icon>exit_to_app</v-icon>
          </v-list-tile-action>
          <v-list-tile-title>Sign out</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>

    <v-snackbar
      v-model="projectLoadingError"
      color="error"
      :multi-line="true"
      :timeout="10000"
    >
      Couldn't reach the server.<br>Are you connected to the internet?
      <v-btn
        dark
        flat
        @click="projectLoadingError = false"
      >
        Close
      </v-btn>
    </v-snackbar>
  </nav>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from '@/tools/axios'
import { mapGetters, mapActions } from 'vuex'
import { ProjectEntity } from '@/models/definitions'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '@/store/modules/projects'
import Component from 'vue-class-component'

@Component({
  props: {
    drawer: {
      type: Boolean,
      default: true
    },
    loadingProjects: {
      type: Boolean,
      default: false
    },
    projectLoadingError: {
      type: Boolean,
      default: false
    },
    projects: {
      type: Array,
      default: () => []
    },
    profileActions: {
      type: Array,
      default: () => [
        ['Personal', 'account_circle', 'configureProjects'],
        ['Spiritual', 'assignment_turned_in', 'configureProjects'],
        ['Availability', 'event_available', 'configureProjects'],
        ['Account', 'lock', 'configureProjects'],
        ['Notifications', 'notifications', 'configureProjects'],
        ['Configure Projects', 'settings', 'configureProjects']
      ]
    }
  }
})
export default class Navbar extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private async created() {
    // TODO: defer this to after sign-in
    await this.projectModule.fetchProjects()
  }

  private get myProjects(): ProjectEntity[] {
    return this.projectModule.myProjects.map((project: any) => {
      project.actions = [
        ['Knowledge Base', 'subject', 'knowledgeBase'],
        ['Calendar', 'today', 'calendar']
      ]
      project.adminActions = [
        ['Settings', 'settings', 'settings'],
        ['User Management', 'group', 'settings'],
        ['Categories', 'label', 'settings'],
        ['Teams', 'supervisor_account', 'settings'],
        ['Meeting Points', 'location_on', 'settings'],
        ['Notifications', 'notifications', 'settings'],
        ['Reports', 'message', 'settings'],
        ['Statistics', 'show_chart', 'settings'],
        ['Notes', 'edit', 'settings']
      ]
      return project
    })
  }
}
</script>

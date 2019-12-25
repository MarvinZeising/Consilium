<template>
  <v-container fluid>
    <v-layout wrap>

      <PersonsPending />

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../../store/projects'
import { Project } from '../../models/definitions'
import PersonsPending from '../../components/PersonsPending.vue'

@Component({
  components: {
    PersonsPending,
  }
})
export default class Persons extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  public get getProject(): Project {
    const projectId = this.$route.params.projectId
    const projects = this.projectModule.getProjects.filter((project) => project.id === projectId)
    return projects.length > 0 ? projects[0] : new Project('Loading', 'Loading')
  }

}
</script>

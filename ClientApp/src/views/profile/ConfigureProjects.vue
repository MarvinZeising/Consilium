<template>
  <v-container fluid>

    <PersonalProjects />

  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import PersonalProjects from '../../components/PersonalProjects.vue'

@Component({
  components: { PersonalProjects }
})
export default class ConfigureProjects extends Vue {}
</script>

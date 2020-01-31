<template>
  <v-flex
    xs12 sm6
    class="pa-2"
  >

    <v-select
      v-if="projectModule.getActiveProject"
      v-model="model.value"
      :items="projectModule.getActiveProject.getCategories"
      :label="$tc('shift.category.categories', 1)"
      item-value="id"
      item-text="name"
      :rules="rules"
      filled
      return-object
    />

  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import { Category } from '../../models'

@Component
export default class CategoryControl extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)

  @Prop(Object)
  private readonly model?: { value?: Category }

  @Prop(Boolean)
  private required = false

  private rules = [
    (v: string) => !this.required
                   || !!v
                   || i18n.t('core.fieldRequired'),
  ]

}
</script>

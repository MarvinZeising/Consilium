<template>
  <v-toolbar-items>

    <v-select
      v-if="projectModule.getActiveProject"
      v-model="model.selected"
      :items="projectModule.getActiveProject.getCategories"
      :label="$tc('shift.category.categories', 2)"
      class="ma-3"
      item-value="id"
      item-text="name"
      multiple
      outlined
      dense
      chips
      small-chips
      return-object
      @change="emitChange"
    >
      <template v-slot:prepend-item>
        <v-list-item
          ripple
          @click="toggle"
        >
          <v-list-item-action>
            <v-icon
              :color="model.selected.length > 0 ? 'primary' : ''"
              v-html="getIcon"
            />
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-t="'core.selectAll'" />
          </v-list-item-content>
        </v-list-item>
        <v-divider class="mt-2"></v-divider>
      </template>
    </v-select>

  </v-toolbar-items>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import { Category } from '../../models'

@Component
export default class CategoriesControl extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)

  @Prop(Object)
  private readonly model?: { selected: Category[] }

  private get getIcon() {
    if (this.model?.selected.length === this.projectModule.getActiveProject?.getCategories.length) {
      return 'check_box'
    } else if (this.model && this.model.selected.length > 0) {
      return 'indeterminate_check_box'
    }
    return 'check_box_outline_blank'
  }

  @Emit('change')
  private emitChange() {
    return this.model?.selected
  }

  private toggle() {
    if (this.model) {
      if (this.model.selected.length === this.projectModule.getActiveProject?.getCategories.length) {
        this.model.selected = []
      } else if (this.model) {
        this.model.selected = this.projectModule.getActiveProject?.getCategories || []
      }
      this.emitChange()
    }
  }

}
</script>

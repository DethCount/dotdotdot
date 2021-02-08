<template>
    <div class="card" v-if="node != null">
        <div class="card-header" id="headingOne">
        <h2 class="mb-0">
            <button
                class="btn dropdown-toggle btn-block text-left"
                type="button"
                v-on:click="showBody = !showBody"
                >
            {{ node[0] }}
            </button>
        </h2>
        </div>
        <div
            v-if="node[1]"
            v-show="showBody"
            >
            <div class="card-body" v-if="typeof node[1].entries == 'function'">
                <Tree v-bind:nodes="node[1].entries()"></Tree>
            </div>
            <div class="card-body leaf" v-else>
                <table class="table table-striped">
                    <thead>
                        <th>Field</th>
                        <th>Value</th>
                    </thead>
                    <tbody>
                        <template v-for="(value, key) in node[1]">
                            <tr v-bind:key="key + 'tr'" v-if="key != 'properties'">
                                <td>{{ key }}</td>
                                <td>{{ value }}</td>
                            </tr>
                        </template>
                    </tbody>
                </table>

                <Tree v-bind:nodes="node[1].properties" v-if="node[1].properties !== undefined"></Tree>
            </div>
        </div>
    </div>
</template>

<script src="@/components/Tree/TreeNode/TreeNode.js"></script>

<style lang="sass" scoped src="@/components/Tree/TreeNode/TreeNode.sass"></style>
